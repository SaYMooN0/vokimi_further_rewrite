using GeneralVokiTakingService.Infrastructure.persistence;
using InfrastructureShared.Base;
using Microsoft.AspNetCore.Builder;

namespace GeneralVokiTakingService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<GeneralVokiTakingDbContext>>();
        return app;
    }
}