using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;

namespace AlbumsService.Api.contracts;

public record AlbumCreatedResponse(
    string Id,
    string Name,
    string Icon,
    string MainColor,
    string SecondColor,
    DateTime CreationDate
) : ICreatableResponse<VokiAlbum>
{

    public static ICreatableResponse<VokiAlbum> Create(VokiAlbum album) => new AlbumCreatedResponse(
        album.Id.ToString(),
        album.Name.ToString(),
        album.Icon.ToString(),
        album.MainColor.ToString(),
        album.SecondaryColor.ToString(),
        album.CreationDate
    );
}