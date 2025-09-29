using VokiRatingsService.Domain.voki_rating_aggregate.events;

namespace VokiRatingsService.Domain.voki_rating_aggregate;

public class VokiRating : AggregateRoot<VokiRatingId>
{
    private VokiRating() { }
    public AppUserId UserId { get; }
    public VokiId VokiId { get; }
    public RatingValueWithDate Current { get; set; }
    public ImmutableArray<RatingValueWithDate> PreviousValues { get; private set; }

    private VokiRating(VokiRatingId id, AppUserId userId, VokiId vokiId, RatingValueWithDate value) {
        Id = id;
        UserId = userId;
        VokiId = vokiId;
        Current = value;
        PreviousValues = [];
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
                $"NewValue.DateTime = {newValue.DateTime:o}, LastUpdated = {Current.DateTime:o}"
            );
        }

        PreviousValues = PreviousValues.Add(Current);
        Current = newValue;

        return ErrOrNothing.Nothing;
    }
}