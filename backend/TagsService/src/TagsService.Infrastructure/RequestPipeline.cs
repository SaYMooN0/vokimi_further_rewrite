using InfrastructureShared;
using Microsoft.AspNetCore.Builder;
using TagsService.Infrastructure.persistence;

namespace TagsService.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrastructureMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<EventualConsistencyMiddleware<TagsDbContext>>();
        return app;
    }
}