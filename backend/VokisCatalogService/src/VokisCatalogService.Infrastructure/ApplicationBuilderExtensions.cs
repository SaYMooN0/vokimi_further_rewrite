using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Builder;
using VokisCatalogService.Infrastructure.persistence;

namespace VokisCatalogService.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<VokisCatalogDbContext>>();
        return app;
    }
}