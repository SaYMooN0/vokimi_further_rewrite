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

    public Task<VokiAlbum[]> ListAlbumsForUserAsNoTracking(AppUserId userId, CancellationToken ct) =>
        _db.VokiAlbums
            .AsNoTracking()
            .Where(a => a.OwnerId == userId)
            .ToArrayAsync(ct);

    public Task<VokiAlbum[]> ListAlbumsForUser(AppUserId userId, CancellationToken ct) =>
        _db.VokiAlbums
            .Where(a => a.OwnerId == userId)
            .ToArrayAsync(ct);

    public Task<VokiAlbumPreviewDto[]> GetPreviewsForUserSortedAsNoTracking(AppUserId userId) =>
        _db.VokiAlbums
            .AsNoTracking()
            .Where(a => a.OwnerId == userId)
            .OrderByDescending(a => a.CreationDate)
            .Select(a => new VokiAlbumPreviewDto(
                a.Id, a.Name, a.Icon, a.MainColor, a.SecondaryColor,
                a.VokiIds.Count, a.CreationDate
            ))
            .ToArrayAsync();

    public async Task Add(VokiAlbum album) {
        await _db.VokiAlbums.AddAsync(album);
        await _db.SaveChangesAsync();
    }

    public async Task<VokiAlbum?> GetById(VokiAlbumId albumId) =>
        await _db.VokiAlbums.FindAsync(albumId);

    public async Task DeleteAlbum(VokiAlbum album) {
        _db.VokiAlbums.Remove(album);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateRange(IEnumerable<VokiAlbum> albums, CancellationToken ct) {
        _db.VokiAlbums.UpdateRange(albums);
        await _db.SaveChangesAsync(ct);
    }
}