using AlbumsService.Domain.voki_album_aggregate;

namespace AlbumsService.Api.contracts;

public record AllUserAlbumsResponse(
    AlbumDataResponse[] Albums
)
{
    public static AllUserAlbumsResponse Create(IReadOnlyCollection<VokiAlbum> albums) => new(
        albums.Select(AlbumDataResponse.Create).ToArray()
    );
}