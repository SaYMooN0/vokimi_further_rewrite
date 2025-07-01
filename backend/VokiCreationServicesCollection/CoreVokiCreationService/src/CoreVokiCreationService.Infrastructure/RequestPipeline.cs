using CoreVokiCreationService.Infrastructure.persistence;
using InfrastructureShared;
using Microsoft.AspNetCore.Builder;

namespace CoreVokiCreationService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<CoreVokiCreationDbContext>>();
        return app;
    }
}