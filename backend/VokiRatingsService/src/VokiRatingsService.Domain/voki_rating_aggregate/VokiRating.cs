using VokiRatingsService.Domain.voki_rating_aggregate.events;

namespace VokiRatingsService.Domain.voki_rating_aggregate;

public class VokiRating : AggregateRoot<VokiRatingId>
{
    private VokiRating() { }
    public AppUserId UserId { get; }
    public VokiId VokiId { get; }
    public RatingValueWithDate Current { get; set; }
    public RatingHistory History { get; }

    private VokiRating(VokiRatingId id, AppUserId userId, VokiId vokiId, RatingValueWithDate value) {
        Id = id;
        UserId = userId;
        VokiId = vokiId;
        Current = value;
        History = RatingHistory.CreateNew();
    }

    public static VokiRating CreateNew(AppUserId userId, VokiId vokiId, RatingValueWithDate value) {
        VokiRating rating = new(VokiRatingId.CreateNew(), userId, vokiId, value);
        rating.AddDomainEvent(new NewVokiRatingCreatedEvent(rating.Id, vokiId, userId));
        return rating;
    }

    public ErrOrNothing Update(RatingValueWithDate newValue) {
        if (newValue.DateTime < Current.DateTime) {
            return ErrFactory.ValueOutOfRange(
                "New rating date cannot be earlier than the last updated date",
                $"New value datetime:{newValue.DateTime:o}. Last updated: {Current.DateTime:o}"
            );
        }

        ErrOrNothing historyErr = History.Add(Current);
        if (historyErr.IsErr(out var err)) {
            return err;
        }

        Current = newValue;
        return ErrOrNothing.Nothing;
    }
}