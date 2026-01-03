using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class VokiRatingsSnapshotRepository : IVokiRatingsSnapshotRepository
{
    private readonly VokiRatingsDbContext _db;

    public VokiRatingsSnapshotRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public Task<VokiRatingsSnapshot?> GetLastSnapshotForVoki(
        VokiId vokiId,
        CancellationToken ct
    ) => _db.VokiRatingsSnapshots
        .Where(s => s.VokiId == vokiId)
        .OrderByDescending(s => s.Date)
        .FirstOrDefaultAsync(ct);


    public async Task Add(VokiRatingsSnapshot snapshot, CancellationToken ct) {
        _db.VokiRatingsSnapshots.Add(snapshot);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Update(VokiRatingsSnapshot snapshot, CancellationToken ct) {
        _db.VokiRatingsSnapshots.Update(snapshot);
        await _db.SaveChangesAsync(ct);
    }

    public Task<VokiRatingsSnapshot[]> ListSortedSnapshotsForVokiAsNoTracking(
        VokiId vokiId,
        CancellationToken ct
    ) =>
        _db.VokiRatingsSnapshots
            .AsNoTracking()
            .Where(s => s.VokiId == vokiId)
            .OrderBy(s => s.Date)
            .ToArrayAsync(ct);
}