using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Builder;
using VokiCommentsService.Infrastructure.persistence;

namespace VokiCommentsService.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<VokiCommentsDbContext>>();
        return app;
    }
}