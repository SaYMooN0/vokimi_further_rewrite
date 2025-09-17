using AlbumsService.Domain.voki_album_aggregate;

namespace AlbumsService.Api.contracts;

public record AlbumDataResponse(
    string Id,
    string Name,
    string Icon,
    string MainColor,
    string SecondColor,
    string[] VokiIds
)
{
    public static AlbumDataResponse Create(VokiAlbum album) => new(
        album.Id.ToString(),
        album.Name.ToString(),
        album.Icon.ToString(),
        album.MainColor.ToString(),
        album.SecondColor.ToString(),
        album.VokiIds.Select(i => i.ToString()).ToArray()
    );
}