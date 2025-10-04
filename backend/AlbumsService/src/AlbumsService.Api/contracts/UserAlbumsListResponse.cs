using AlbumsService.Domain.voki_album_aggregate;

namespace AlbumsService.Api.contracts;

public record UserAlbumsListResponse(
    AlbumDataResponse[] Albums
)
{
    public static UserAlbumsListResponse Create(IReadOnlyCollection<VokiAlbum> albums) => new(
        albums.Select(AlbumDataResponse.FromAlbum).ToArray()
    );
}