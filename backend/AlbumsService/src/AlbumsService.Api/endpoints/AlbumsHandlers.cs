using AlbumsService.Api.contracts;
using ApiShared.extensions;

namespace AlbumsService.Api.endpoints;

internal static class AlbumsHandlers
{
    internal static void MapAlbumsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/")
            .WithGroupAuthenticationRequired();

        // group.MapGet("/user-albums-data", GetUserAlbumsData);
        //
        // group.MapPost("/create-new", CreateNewVokiAlbum)
        //     .WithRequestValidation<CreateNewVokiAlbumRequest>();
        //
        // group.MapPatch("/update-voki-entries", UpdateVokiEntriesInAlbums)
        //     .WithRequestValidation<UpdateVokiEntriesInAlbumsRequest>();
    }
}