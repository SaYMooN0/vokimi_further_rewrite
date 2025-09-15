using ApiShared.extensions;

namespace AlbumsService.Api.endpoints;

internal static class SpecificAlbumHandlers
{
    internal static void MapSpecificAlbumHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/")
            .WithGroupAuthenticationRequired();

        // group.MapPatch("/update", UpdateAlbum);
    }
}