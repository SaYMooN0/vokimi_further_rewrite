using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using SharedKernel.errs.utils;

namespace ApiShared.EfCore;

public sealed class EventualConsistencyEndpointFilter<TDbContext> : IEndpointFilter
    where TDbContext : DbContext
{
    private readonly ILogger<EventualConsistencyEndpointFilter<TDbContext>> _logger;

    public EventualConsistencyEndpointFilter(
        ILogger<EventualConsistencyEndpointFilter<TDbContext>> logger
    ) {
        _logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    ) {
        var httpContext = context.HttpContext;

        Endpoint? endpoint = httpContext.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<DisableConsistencyFilterMetadata>() is not null) {
            _logger.LogDebug(
                "Handle request {Path} with no transaction because of {DisableConsistencyFilterMetadata}",
                httpContext.Request.Path,
                nameof(DisableConsistencyFilterMetadata)
            );
            return await next(context);
        }

        var dbContext = httpContext.RequestServices.GetRequiredService<TDbContext>();

        await using IDbContextTransaction
            transaction = await dbContext.Database.BeginTransactionAsync(httpContext.RequestAborted);
        _logger.LogDebug(
            "Started transaction {TransactionId} for request {Path}",
            transaction.TransactionId,
            httpContext.Request.Path
        );

        try {
            var result = await next(context);

            await transaction.CommitAsync(httpContext.RequestAborted);

            _logger.LogDebug(
                "Committed transaction {TransactionId} for request {Path}",
                transaction.TransactionId,
                httpContext.Request.Path
            );

            return result;
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.LockNotAvailable) {
            // FOR UPDATE NOWAIT -> row is locked
            _logger.LogInformation(
                "Transaction {TransactionId} failed due to lock contention for request {Path}",
                transaction.TransactionId,
                httpContext.Request.Path
            );

            await transaction.RollbackAsync(httpContext.RequestAborted);

            return CustomResults.ErrorResponse(ErrFactory.Conflict(
                "Resource is currently being modified by another request"
            ));
        }
        catch (OperationCanceledException) when (httpContext.RequestAborted.IsCancellationRequested) {
            _logger.LogWarning(
                "Request {Path} was aborted, rolling back transaction {TransactionId}",
                httpContext.Request.Path,
                transaction.TransactionId
            );

            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
        catch (Exception ex) {
            _logger.LogError(
                ex,
                "Error during request {Path}, rolling back transaction {TransactionId}",
                httpContext.Request.Path,
                transaction.TransactionId
            );

            await transaction.RollbackAsync(httpContext.RequestAborted);
            throw;
        }
    }
}