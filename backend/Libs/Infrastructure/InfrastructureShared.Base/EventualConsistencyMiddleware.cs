using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureShared.Base;

public class EventualConsistencyMiddleware<T> where T : DbContext
{
    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, T dbContext) {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        context.Response.OnCompleted(async () => {
            try {
                await transaction.CommitAsync();
            }
            catch (Exception ex) {
                await transaction.RollbackAsync();
            }
            finally {
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }
}