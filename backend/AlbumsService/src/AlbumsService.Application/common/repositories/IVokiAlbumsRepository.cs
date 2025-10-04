using AlbumsService.Domain.voki_album_aggregate;

namespace AlbumsService.Application.common.repositories;

public interface IVokiAlbumsRepository
{
    Task<VokiAlbum[]> GetForUserSortedAsNoTracking(AppUserId userId);
    Task<VokiAlbumPreviewDto[]> GetPreviewsForUserSortedAsNoTracking(AppUserId userId);
    Task Add(VokiAlbum album);
    Task<VokiAlbum?> GetById(VokiAlbumId albumId);
    Task DeleteAlbum(VokiAlbum album);
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