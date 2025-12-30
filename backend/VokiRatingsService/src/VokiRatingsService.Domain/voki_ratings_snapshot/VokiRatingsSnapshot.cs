using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_ratings_snapshot.events;

namespace VokiRatingsService.Domain.voki_ratings_snapshot;

public class VokiRatingsSnapshot : AggregateRoot<VokiRatingsSnapshotId>
{
    private VokiRatingsSnapshot() { }
    public VokiId VokiId { get; }
    public DateTime Date { get; private set; }
    public VokiRatingsDistribution Distribution { get; private set; }

    private VokiRatingsSnapshot(VokiRatingsSnapshotId id, VokiId vokiId, DateTime now, VokiRatingsDistribution distribution) {
        Id = id;
        VokiId = vokiId;
        Date = now;
        Distribution = distribution;
    }

    public static VokiRatingsSnapshot CreateNew(VokiId vokiId, DateTime now, VokiRatingsDistribution distribution) => new(
        VokiRatingsSnapshotId.CreateNew(), vokiId, now, distribution
    );

    public void Update(DateTime now, VokiRatingsDistribution distribution) {
        var oldRatingsCount = this.Distribution.TotalCount;
        Date = now;
        Distribution = distribution;

        var newRatingsCount = this.Distribution.TotalCount;
        if (oldRatingsCount != newRatingsCount) {
            AddDomainEvent(new VokiRatingsChangedCount(VokiId, newRatingsCount));
        }
    }
}