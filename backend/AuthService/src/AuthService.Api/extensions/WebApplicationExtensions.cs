namespace AuthService.Api.extensions;

public static class WebApplicationExtensions
{
    internal static void MapEndpoints(this WebApplication app) {
        // app.MapRootHandlers();
    }

    internal static void AllowFrontendCors(this WebApplication app) => app.UseCors("AllowFrontend");
}