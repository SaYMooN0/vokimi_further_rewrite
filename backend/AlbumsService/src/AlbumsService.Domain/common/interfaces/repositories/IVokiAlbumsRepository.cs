using AlbumsService.Domain.voki_album_aggregate;

namespace AlbumsService.Domain.common.interfaces.repositories;

public interface IVokiAlbumsRepository
{
    Task<VokiAlbum[]> GetForUserSortedAsNoTracking(AppUserId userId);
    Task Add(VokiAlbum album);
    Task<VokiAlbum?> GetById(VokiAlbumId albumId);
    Task DeleteAlbum(VokiAlbum album);
}