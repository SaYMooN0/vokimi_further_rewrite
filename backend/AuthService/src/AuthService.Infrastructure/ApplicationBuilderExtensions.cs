using AuthService.Infrastructure.persistence;
using InfrastructureShared.Base;
using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Builder;

namespace AuthService.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.AddEventualConsistencyMiddleware<AuthDbContext>();
        return app;
    }
}