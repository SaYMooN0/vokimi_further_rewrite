using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using InfrastructureShared.EfCore.query_extensions;
using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;
using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class VokiRatingsSnapshotRepository : IVokiRatingsSnapshotRepository
{
    private readonly VokiRatingsDbContext _db;

    public VokiRatingsSnapshotRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public async Task<Dictionary<VokiId, VokiRatingsSnapshot>>
        GetLastSnapshotForVokisAsTracking(
            IEnumerable<VokiId> vokiIds,
            CancellationToken ct
        ) {
        var ids = vokiIds.ToArray();

        var snapshots = await _db.VokiRatingsSnapshots
            .Where(x => ids.Contains(x.VokiId))
            .GroupBy(x => x.VokiId)
            .Select(g => g
                .OrderByDescending(x => x.Date)
                .First())
            .AsTracking()
            .ToListAsync(ct);

        return snapshots.ToDictionary(x => x.VokiId);
    }


    public async Task Add(VokiRatingsSnapshot snapshot, CancellationToken ct) {
        _db.VokiRatingsSnapshots.Add(snapshot);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Update(VokiRatingsSnapshot snapshot, CancellationToken ct) {
        _db.ThrowIfDetached(snapshot);
        _db.VokiRatingsSnapshots.Update(snapshot);
        await _db.SaveChangesAsync(ct);
    }

    public Task<VokiRatingsSnapshot[]> ListSortedSnapshotsForVoki(VokiId vokiId, CancellationToken ct) =>
        _db.VokiRatingsSnapshots
            .Where(s => s.VokiId == vokiId)
            .OrderBy(s => s.Date)
            .ToArrayAsync(ct);
}