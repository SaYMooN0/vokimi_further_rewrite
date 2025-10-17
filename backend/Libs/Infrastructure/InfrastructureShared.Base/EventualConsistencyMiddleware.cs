using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
namespace InfrastructureShared.Base;

public class EventualConsistencyMiddleware<T> where T : DbContext
{
    private readonly RequestDelegate _next;
    private readonly ILogger<EventualConsistencyMiddleware<T>> _logger;

    public EventualConsistencyMiddleware(
        RequestDelegate next,
        ILogger<EventualConsistencyMiddleware<T>> logger
    ) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, T dbContext) {
        IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();

        context.Response.OnCompleted(async () => {
            try {
                await transaction.CommitAsync();
                _logger.LogDebug(
                    "Committed transaction {TransactionId} for request {Path}",
                    transaction.TransactionId, context.Request.Path
                );
            }
            catch (Exception ex) {
                _logger.LogError(
                    ex, "Error committing transaction {TransactionId} for request {Path}, rolling back",
                    transaction.TransactionId, context.Request.Path);
                try {
                    await transaction.RollbackAsync();
                    _logger.LogDebug(
                        "Rolled back transaction {TransactionId} for request {Path}",
                        transaction.TransactionId, context.Request.Path
                    );
                }
                catch (Exception rbEx) {
                    _logger.LogError(
                        rbEx, "Rollback failed for transaction {TransactionId} after error in request {Path}",
                        transaction.TransactionId, context.Request.Path
                    );
                }
            }
            finally {
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }
}