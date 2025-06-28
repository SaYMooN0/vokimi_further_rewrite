using AuthService.Infrastructure.persistence;
using InfrastructureShared;
using Microsoft.AspNetCore.Builder;

namespace AuthService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<AuthDbContext>>();
        return app;
    }
}