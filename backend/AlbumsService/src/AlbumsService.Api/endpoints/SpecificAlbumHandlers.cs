using ApiShared;
using ApiShared.extensions;

namespace AlbumsService.Api.endpoints;

internal  class SpecificAlbumHandlers : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/albums/{albumId}")
            .WithGroupAuthenticationRequired();
        
        // group.MapPatch("/update", UpdateAlbum);
        // group.MapDelete("/delete", DeleteAlbum);
    }
}