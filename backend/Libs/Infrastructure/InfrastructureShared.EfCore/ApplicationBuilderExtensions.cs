using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureShared.EfCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddEventualConsistencyMiddleware<T>(this IApplicationBuilder app) where T : DbContext {
        app.UseMiddleware<EventualConsistencyMiddleware<T>>();
        return app;
    }
}