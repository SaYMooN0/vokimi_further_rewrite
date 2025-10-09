using AlbumsService.Api.endpoints;

namespace AlbumsService.Api.extensions;

public static class WebApplicationExtensions
{
    internal static void AllowFrontendCors(this WebApplication app) => app.UseCors("AllowFrontend");

    internal static void MapEndpoints(this WebApplication app) {
        app.MapAlbumsHandlers();
        app.MapSpecificAlbumHandlers();
        app.MapSpecificVokiHandlers();
    }
}