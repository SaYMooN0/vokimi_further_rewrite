using InfrastructureShared;
using Microsoft.AspNetCore.Builder;
using VokisCatalogService.Infrastructure.persistence;

namespace VokisCatalogService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<VokisCatalogTakingDbContext>>();
        return app;
    }
}