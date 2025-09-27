namespace VokiRatingsService.Domain.voki_aggregate;

public class Voki : AggregateRoot<VokiId>
{
    private Voki() { }
    public ImmutableHashSet<VokiRatingId> Ratings { get; private set; }

    public Voki(VokiId id) {
        Id = id;
        Ratings = [];
    }

    public void AddRating(VokiRatingId vokiRatingId) {
        Ratings = Ratings.Add(vokiRatingId);
    }
}