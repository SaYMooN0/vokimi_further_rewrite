using GeneralVokiCreationService.Api.endpoints;

namespace GeneralVokiCreationService.Api.extensions;

public static class WebApplicationExtensions
{
    internal static void AllowFrontendCors(this WebApplication app) => app.UseCors("AllowFrontend");

    internal static void MapEndpoints(this WebApplication app) {
        app.MapRootHandlers();
        app.MapSpecificVokiHandlers();
        app.MapVokiQuestionsHandlers();
        app.MapSpecificVokiQuestionsHandlers();
    }
}