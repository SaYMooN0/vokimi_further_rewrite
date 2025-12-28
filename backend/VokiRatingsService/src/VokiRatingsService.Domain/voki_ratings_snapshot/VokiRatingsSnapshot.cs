using VokiRatingsService.Domain.voki_aggregate;
using VokiRatingsService.Domain.voki_ratings_snapshot.events;

namespace VokiRatingsService.Domain.voki_ratings_snapshot;

public class VokiRatingsSnapshot : AggregateRoot<VokiRatingsSnapshotId>
{
    private VokiRatingsSnapshot(VokiRatingsSnapshotId id, VokiId vokiId, DateTime now, VokiRatingsDistribution distribution) {
        Id = id;
        VokiId = vokiId;
        Date = now;
        Distribution = distribution;
        Average = Distribution.TotalSum == 0
            ? 0
            : (double)Distribution.TotalSum / Distribution.TotalCount;
    }

    public VokiId VokiId { get; }
    public DateTime Date { get; private set; }
    public VokiRatingsDistribution Distribution { get; private set; }
    public double Average { get; private set; }

    public static VokiRatingsSnapshot CreateNew(VokiId vokiId, DateTime now, VokiRatingsDistribution distribution) => new(
        VokiRatingsSnapshotId.CreateNew(), vokiId, now, distribution
    );

    public void Update(DateTime now, VokiRatingsDistribution distribution) {
        var oldRatingsCount = this.Distribution.TotalCount;
        Date = now;
        Distribution = distribution;
        Average = Distribution.TotalSum == 0
            ? 0
            : (double)Distribution.TotalSum / Distribution.TotalCount;

        var newRatingsCount = this.Distribution.TotalCount;
        if (oldRatingsCount != newRatingsCount) {
            AddDomainEvent(new VokiRatingsChangedCount(VokiId, newRatingsCount));
        }
    }
}