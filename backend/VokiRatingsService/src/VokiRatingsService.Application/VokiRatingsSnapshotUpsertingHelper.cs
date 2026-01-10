using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Application;

internal static class VokiRatingsSnapshotUpsertingHelper
{
    public static async Task UpsertDailySnapshotOnRatingCreated(
        VokiId vokiId,
        RatingValue newRating,
        DateTime now,
        IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository,
        IRatingsRepository ratingsRepository,
        CancellationToken ct
    ) {
        VokiRatingsSnapshot? lastSnapshot = await vokiRatingsSnapshotRepository.GetLastSnapshotForVokiForUpdate(vokiId, ct);
        if (lastSnapshot is null) {
            await CreateFirstSnapshotForVoki(vokiId, newRating, now, vokiRatingsSnapshotRepository, ct);
        }
        else {
            await UpdateExistingOrCreateNewSnapshot(
                lastSnapshot, now, (currentDistribution) => currentDistribution.WithNewRating(newRating),
                vokiRatingsSnapshotRepository, ratingsRepository, ct
            );
        }
    }


    public static async Task UpsertDailySnapshotOnRatingUpdated(
        VokiId vokiId,
        DateTime now,
        RatingValue oldValue,
        RatingValue newValue,
        IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository,
        IRatingsRepository ratingsRepository,
        CancellationToken ct
    ) {
        VokiRatingsSnapshot? lastSnapshot = await vokiRatingsSnapshotRepository.GetLastSnapshotForVokiForUpdate(vokiId, ct);
        if (lastSnapshot is null) {
            await CreateFirstSnapshotForVoki(vokiId, newValue, now, vokiRatingsSnapshotRepository, ct);
        }
        else {
            await UpdateExistingOrCreateNewSnapshot(
                lastSnapshot, now,
                (currentDistribution) => {
                    var updatingRes = currentDistribution.WithOneUpdatedRating(newRating: newValue, oldRating: oldValue);
                    UnexpectedBehaviourException.ThrowIfErr(updatingRes);
                    return updatingRes.AsSuccess();
                },
                vokiRatingsSnapshotRepository, ratingsRepository, ct
            );
        }
    }

    private static async Task CreateFirstSnapshotForVoki(
        VokiId vokiId,
        RatingValue firstRating,
        DateTime now,
        IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository,
        CancellationToken ct
    ) {
        VokiRatingsDistribution distribution = VokiRatingsDistribution.FromRating(firstRating);
        VokiRatingsSnapshot newSnapshot = VokiRatingsSnapshot.CreateNew(vokiId, now, distribution);
        await vokiRatingsSnapshotRepository.Add(newSnapshot, ct);
    }

    private static async Task UpdateExistingOrCreateNewSnapshot(
        VokiRatingsSnapshot lastSnapshot, DateTime now,
        Func<VokiRatingsDistribution, VokiRatingsDistribution> gatherNewDistribution,
        IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository, IRatingsRepository ratingsRepository,
        CancellationToken ct
    ) {
        VokiRatingsDistribution currentDistribution =
            await ratingsRepository.GetRatingsDistributionForVoki(lastSnapshot.VokiId, ct);
        var distributionWithNewRating = gatherNewDistribution(currentDistribution);
        if (lastSnapshot.IsInSameDayAs(now)) {
            lastSnapshot.Update(now, distributionWithNewRating);
            await vokiRatingsSnapshotRepository.Update(lastSnapshot, ct);
        }
        else {
            VokiRatingsSnapshot newSnapshot = VokiRatingsSnapshot.CreateNew(lastSnapshot.VokiId, now, distributionWithNewRating);
            await vokiRatingsSnapshotRepository.Update(newSnapshot, ct);
        }
    }
}