using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;

namespace AlbumsService.Api.contracts;

public record AlbumDataResponse(
    string Id,
    string Name,
    string Icon,
    string MainColor,
    string SecondColor,
    string[] VokiIds,
    DateTime CreationDate
) : ICreatableResponse<VokiAlbum>
{
    public static AlbumDataResponse FromAlbum(VokiAlbum album) => new AlbumDataResponse(
        album.Id.ToString(),
        album.Name.ToString(),
        album.Icon.ToString(),
        album.MainColor.ToString(),
        album.SecondaryColor.ToString(),
        album.VokiIds.Select(i => i.ToString()).ToArray(),
        album.CreationDate
    );

    public static ICreatableResponse<VokiAlbum> Create(VokiAlbum album) => FromAlbum(album);
}