using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Application;

internal static class VokiRatingsSnapshotUpsertingHelper
{
    public static async Task UpsertDailySnapshot(
        VokiId vokiId,
        DateTime now,
        IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository,
        IRatingsRepository ratingsRepository,
        CancellationToken ct
    ) {
        VokiRatingsSnapshot? lastSnapshot = await vokiRatingsSnapshotRepository.GetLastSnapshotForVokiInThisDay(
            vokiId, DateOnly.FromDateTime(now), ct
        );
        VokiRatingsDistribution currentDistribution = await ratingsRepository.GetRatingsDistributionForVoki(vokiId, ct);
        if (lastSnapshot is null) {
            VokiRatingsSnapshot newSnapshot = VokiRatingsSnapshot.CreateNew(vokiId, now, currentDistribution);
            await vokiRatingsSnapshotRepository.Add(newSnapshot, ct);
        }
        else {
            lastSnapshot.Update(now, currentDistribution);
            await vokiRatingsSnapshotRepository.Update(lastSnapshot, ct);
        }
    }
}