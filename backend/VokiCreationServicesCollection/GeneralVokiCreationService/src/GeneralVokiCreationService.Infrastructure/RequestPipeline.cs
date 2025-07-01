using GeneralVokiCreationService.Infrastructure.persistence;
using InfrastructureShared;
using Microsoft.AspNetCore.Builder;

namespace GeneralVokiCreationService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<GeneralVokiCreationDbContext>>();
        return app;
    }
}