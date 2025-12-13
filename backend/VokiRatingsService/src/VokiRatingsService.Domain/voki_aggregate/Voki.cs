namespace VokiRatingsService.Domain.voki_aggregate;

public class Voki : AggregateRoot<VokiId>
{
    private Voki() { }
    public ImmutableHashSet<VokiRatingId> RatingIds { get; private set; }
    protected VokiManagersIdsSet ManagersSet { get; private set; }


    public Voki(VokiId id, VokiManagersIdsSet managers) {
        Id = id;
        RatingIds = [];
        ManagersSet = managers;
    }

    public void AddRating(VokiRatingId vokiRatingId) {
        RatingIds = RatingIds.Add(vokiRatingId);
    }
}