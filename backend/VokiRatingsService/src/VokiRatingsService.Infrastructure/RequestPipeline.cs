using InfrastructureShared.Base;
using Microsoft.AspNetCore.Builder;
using VokiRatingsService.Infrastructure.persistence;

namespace VokiRatingsService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<VokiRatingsDbContext>>();
        return app;
    }
}