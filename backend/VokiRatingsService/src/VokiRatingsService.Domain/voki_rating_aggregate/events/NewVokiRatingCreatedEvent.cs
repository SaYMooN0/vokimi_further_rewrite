namespace VokiRatingsService.Domain.voki_rating_aggregate.events;

public record NewVokiRatingCreatedEvent(
    VokiRatingId RatingId,
    VokiId VokiId,
    AppUserId UserId
) : IDomainEvent;