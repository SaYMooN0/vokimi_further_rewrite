using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.update_ratings_snapshot_marker_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class UpdateRatingsSnapshotMarkerRepository : IUpdateRatingsSnapshotMarkerRepository
{
    private readonly VokiRatingsDbContext _db;

    public UpdateRatingsSnapshotMarkerRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public Task<UpdateRatingsSnapshotMarker[]> GetBatch(int limit, CancellationToken ct) {
        return _db.UpdateRatingsSnapshotMarkers
            .Take(limit)
            .ToArrayAsync(ct);
    }

    public async Task Add(UpdateRatingsSnapshotMarker marker, CancellationToken ct) {
        _db.UpdateRatingsSnapshotMarkers.Add(marker);
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> ExistsForVoki(VokiId vokiId, CancellationToken ct) {
        return _db.UpdateRatingsSnapshotMarkers.AnyAsync(m => m.VokiId == vokiId, ct);
    }

    public async Task DeleteBatch(IEnumerable<UpdateRatingsSnapshotMarker> markers, CancellationToken ct) {
        _db.UpdateRatingsSnapshotMarkers.RemoveRange(markers);
        await _db.SaveChangesAsync(ct);
    }
}