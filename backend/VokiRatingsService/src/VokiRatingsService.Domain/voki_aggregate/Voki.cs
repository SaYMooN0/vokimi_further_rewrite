namespace VokiRatingsService.Domain.voki_aggregate;

public class Voki : AggregateRoot<VokiId>
{
    private Voki() { }
    public ImmutableHashSet<VokiRatingId> RatingIds { get; private set; }

    public Voki(VokiId id) {
        Id = id;
        RatingIds = [];
    }

    public void AddRating(VokiRatingId vokiRatingId) {
        RatingIds = RatingIds.Add(vokiRatingId);
    }
}