using CoreVokiCreationService.Infrastructure.persistence;
using InfrastructureShared.Base;
using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Builder;

namespace CoreVokiCreationService.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<CoreVokiCreationDbContext>>();
        return app;
    }
}