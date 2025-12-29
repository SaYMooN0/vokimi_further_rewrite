using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Builder;
using UserProfilesService.Infrastructure.persistence;

namespace UserProfilesService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<UserProfilesDbContext>>();
        return app;
    }
}