using SharedKernel.exceptions;
using VokiRatingsService.Domain.voki_rating_aggregate.events;

namespace VokiRatingsService.Domain.voki_rating_aggregate;

public class VokiRating : AggregateRoot<VokiRatingId>
{
    private VokiRating() { }
    public AppUserId UserId { get; }
    public VokiId VokiId { get; }
    public RatingValue CurrentValue { get; private set; }
    public DateTime LastUpdated { get; private set; }
    public DateTime InitialDateTime { get; }
    public RatingValue InitialValue { get; }
    public bool WasUpdated => LastUpdated != InitialDateTime;

    private VokiRating(VokiRatingId id, AppUserId userId, VokiId vokiId, RatingValue value, DateTime now) {
        Id = id;
        UserId = userId;
        VokiId = vokiId;

        CurrentValue = value;
        InitialValue = value;

        LastUpdated = now;
        InitialDateTime = now;
    }

    public static VokiRating CreateNew(AppUserId userId, VokiId vokiId, RatingValue value, DateTime now) {
        VokiRating rating = new(VokiRatingId.CreateNew(), userId, vokiId, value, now);
        rating.AddDomainEvent(new NewVokiRatingCreatedEvent(rating.Id, vokiId, userId));
        return rating;
    }

    public ErrOrNothing Update(RatingValue newValue, DateTime now) {
        if (now < InitialDateTime) {
            UnexpectedBehaviourException.ThrowErr(
                ErrFactory.Conflict(
                    $"VokiRating has InitialDateTime in the future. Rating id: {Id}. Voki id: {VokiId}. User id: {UserId}. InitialDateTime: {InitialDateTime}. Now: {now}"
                ),
                userMessage: "Cannot update rating because of dates conflict"
            );
        }

        if (now < LastUpdated) {
            UnexpectedBehaviourException.ThrowErr(
                ErrFactory.Conflict(
                    $"VokiRating has LastUpdated in the future. Rating id: {Id}. Voki id: {VokiId}. User id: {UserId}. LastUpdated: {LastUpdated}. Now: {now}"
                ),
                userMessage: "Cannot update rating because of dates conflict"
            );
        }

        // no-op update
        if (newValue == CurrentValue) {
            return ErrOrNothing.Nothing;
        }

        CurrentValue = newValue;
        LastUpdated = now;
        AddDomainEvent(new VokiRatingUpdatedEvent(VokiId));
        return ErrOrNothing.Nothing;
    }
}