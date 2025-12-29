using AlbumsService.Infrastructure.persistence;
using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Builder;

namespace AlbumsService.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<AlbumsDbContext>>();
        return app;
    }
}