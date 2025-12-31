using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Application.common.repositories;

public interface IVokiRatingsSnapshotRepository
{
    Task<VokiRatingsSnapshot?> GetLastSnapshotForVokiInThisDay(VokiId vokiId, DateOnly date, CancellationToken ct);
    Task Add(VokiRatingsSnapshot snapshot, CancellationToken ct);
    Task Update(VokiRatingsSnapshot snapshot, CancellationToken ct);
    Task<VokiRatingsSnapshot[]> ListSortedSnapshotsForVokiAsNoTracking(VokiId vokiId, CancellationToken ct);
}