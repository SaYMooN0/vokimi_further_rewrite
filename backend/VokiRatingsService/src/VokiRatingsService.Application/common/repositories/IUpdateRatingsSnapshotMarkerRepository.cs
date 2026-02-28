using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.update_ratings_snapshot_marker_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IUpdateRatingsSnapshotMarkerRepository
{
    Task<UpdateRatingsSnapshotMarker[]> GetBatch(int limit, CancellationToken ct);
    Task Add(UpdateRatingsSnapshotMarker marker, CancellationToken ct);
    Task<bool> ExistsForVoki(VokiId vokiId, CancellationToken ct);
    Task DeleteBatch(IEnumerable<UpdateRatingsSnapshotMarker> markers, CancellationToken ct);
}