namespace CoreVokiCreationService.Api.extensions;

public static class WebApplicationExtensions
{
    internal static void AllowFrontendCors(this WebApplication app) => app.UseCors("AllowFrontend");

    internal static void MapEndpoints(this WebApplication app) {
        //... mapping endpoints
    }
}