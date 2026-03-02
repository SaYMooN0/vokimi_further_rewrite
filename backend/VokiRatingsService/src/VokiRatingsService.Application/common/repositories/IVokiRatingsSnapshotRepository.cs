using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IVokiRatingsSnapshotRepository
{
    Task Add(VokiRatingsSnapshot snapshot, CancellationToken ct);
    Task<VokiRatingsSnapshot[]> ListSortedSnapshotsForVoki(VokiId vokiId, CancellationToken ct);

    Task<Dictionary<VokiId, VokiRatingsSnapshot>> GetLastSnapshotForVokisAsTracking(
        IEnumerable<VokiId> vokiIds,
        CancellationToken cancellationToken
    );

    Task UpdateRange(IEnumerable<VokiRatingsSnapshot> snapshots, CancellationToken ct);
    Task AddRange(IEnumerable<VokiRatingsSnapshot> snapshots, CancellationToken ct);
}