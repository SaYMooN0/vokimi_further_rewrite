using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;

namespace AlbumsService.Api.contracts.create_new_album;

public record AlbumCreatedResponse(
    string CreateAlbumId
) : ICreatableResponse<VokiAlbum>
{
    public static ICreatableResponse<VokiAlbum> Create(VokiAlbum album) => new AlbumCreatedResponse(
        album.Id.ToString()
    );
}