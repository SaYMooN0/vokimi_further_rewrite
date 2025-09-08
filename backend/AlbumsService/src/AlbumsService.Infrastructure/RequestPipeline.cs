using AlbumsService.Infrastructure.persistence;
using InfrastructureShared.Base;
using Microsoft.AspNetCore.Builder;

namespace AlbumsService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<AlbumsDbContext>>();
        return app;
    }
}