using InfrastructureShared.Base;
using Microsoft.AspNetCore.Builder;
using VokiCommentsService.Infrastructure.persistence;

namespace VokiCommentsService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<VokiCommentsDbContext>>();
        return app;
    }
}