using GeneralVokiCreationService.Infrastructure.persistence;
using InfrastructureShared.Base;
using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Builder;

namespace GeneralVokiCreationService.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<GeneralVokiCreationDbContext>>();
        return app;
    }
}