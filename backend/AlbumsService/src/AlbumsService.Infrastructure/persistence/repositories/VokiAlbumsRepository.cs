using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using Microsoft.EntityFrameworkCore;

namespace AlbumsService.Infrastructure.persistence.repositories;

public class VokiAlbumsRepository : IVokiAlbumsRepository
{
    private readonly AlbumsDbContext _db;

    public VokiAlbumsRepository(AlbumsDbContext db) {
        _db = db;
    }

    public Task<VokiAlbum[]> GetForUserSortedAsNoTracking(AppUserId userId) => _db.VokiAlbums
        .AsNoTracking()
        .Where(a => a.OwnerId == userId)
        .OrderByDescending(a => a.CreationDate)
        .ToArrayAsync();

    public async Task Add(VokiAlbum album) {
        await _db.VokiAlbums.AddAsync(album);
        await _db.SaveChangesAsync();
    }

    public async Task<VokiAlbum?> GetById(VokiAlbumId albumId) => await _db.VokiAlbums.FindAsync(albumId);

    public async Task DeleteAlbum(VokiAlbum album) {
        _db.Remove(album);
        await _db.SaveChangesAsync();
    }
}