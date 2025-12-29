using GeneralVokiTakingService.Infrastructure.persistence;
using InfrastructureShared.Base;
using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Builder;

namespace GeneralVokiTakingService.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<GeneralVokiTakingDbContext>>();
        return app;
    }
}