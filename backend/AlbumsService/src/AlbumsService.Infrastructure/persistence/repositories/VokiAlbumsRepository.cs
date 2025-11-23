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

    public Task<VokiAlbumPreviewDto[]> GetPreviewsForUserSortedAsNoTracking(AppUserId userId, CancellationToken ct) =>
        _db.VokiAlbums
            .AsNoTracking()
            .Where(a => a.OwnerId == userId)
            .OrderByDescending(a => a.CreationDate)
            .Select(a => new VokiAlbumPreviewDto(
                a.Id, a.Name, a.Icon, a.MainColor, a.SecondaryColor,
                a.VokiIds.Count, a.CreationDate
            ))
            .ToArrayAsync(cancellationToken: ct);

    public async Task Add(VokiAlbum album, CancellationToken ct) {
        await _db.VokiAlbums.AddAsync(album, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<VokiAlbum?> GetById(VokiAlbumId albumId, CancellationToken ct) =>
        await _db.VokiAlbums.FindAsync([albumId], cancellationToken: ct);

    public async Task DeleteAlbum(VokiAlbum album, CancellationToken ct) {
        _db.VokiAlbums.Remove(album);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateRange(IEnumerable<VokiAlbum> albums, CancellationToken ct) {
        _db.VokiAlbums.UpdateRange(albums);
        await _db.SaveChangesAsync(ct);
    }

    public Task<VokiAlbum?> GetByIdAsNoTracking(VokiAlbumId albumId, CancellationToken ct) =>
        _db.VokiAlbums.FirstOrDefaultAsync(v => v.Id == albumId, ct);

    public async Task Update(VokiAlbum album, CancellationToken ct) {
        _db.VokiAlbums.Update(album);
        await _db.SaveChangesAsync(ct);
    }
}