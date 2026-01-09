using AlbumsService.Domain.voki_album_aggregate;

namespace AlbumsService.Application.common.repositories;

public interface IVokiAlbumsRepository
{
    Task<VokiAlbum[]> ListAlbumsForUser(AppUserId userId, CancellationToken ct);
    Task<VokiAlbum[]> ListAlbumsForUserForUpdate(AppUserId userId, CancellationToken ct);
    Task<VokiAlbumPreviewDto[]> GetPreviewsForUserSorted(AppUserId userId, CancellationToken ct);
    Task Add(VokiAlbum album, CancellationToken ct);
    Task<VokiAlbum?> GetByIdForUpdate(VokiAlbumId albumId, CancellationToken ct);
    Task DeleteAlbum(VokiAlbum album, CancellationToken ct);
    Task UpdateRange(IEnumerable<VokiAlbum> albums, CancellationToken ct);
    Task<VokiAlbum?> GetById(VokiAlbumId albumId, CancellationToken ct);
    Task Update(VokiAlbum album, CancellationToken ct);
    Task<VokiAlbum[]> ListByIds(IEnumerable<VokiAlbumId> ids, CancellationToken ct);
}

public record VokiAlbumPreviewDto(
    VokiAlbumId Id,
    AlbumName Name,
    AlbumIcon Icon,
    HexColor MainColor,
    HexColor SecondaryColor,
    int VokiIdsCount,
    DateTime CreationDate
);