using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;
using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Application.common.repositories;

public interface IVokiRatingsSnapshotRepository
{
    Task Add(VokiRatingsSnapshot snapshot, CancellationToken ct);
    Task Update(VokiRatingsSnapshot snapshot, CancellationToken ct);
    Task<VokiRatingsSnapshot[]> ListSortedSnapshotsForVoki(VokiId vokiId, CancellationToken ct);

    Task<Dictionary<VokiId, VokiRatingsSnapshot>> GetLastSnapshotForVokisAsTracking(
        IEnumerable<VokiId> vokiIds,
        CancellationToken cancellationToken
    );
}