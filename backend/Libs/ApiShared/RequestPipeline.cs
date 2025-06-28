using ApiShared.middlewares;
using Microsoft.AspNetCore.Builder;

namespace ApiShared;

public static class RequestPipeline
{
    public static IApplicationBuilder AddExceptionHandlingMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}
