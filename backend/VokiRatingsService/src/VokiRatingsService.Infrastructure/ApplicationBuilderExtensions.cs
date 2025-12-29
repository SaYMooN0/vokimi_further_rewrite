using InfrastructureShared.Base;
using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Builder;
using VokiRatingsService.Infrastructure.persistence;

namespace VokiRatingsService.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<VokiRatingsDbContext>>();
        return app;
    }
}