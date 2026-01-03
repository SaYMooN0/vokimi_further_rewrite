using VokiRatingsService.Domain.common;

namespace VokiRatingsService.Domain.voki_rating_aggregate.events;

public record NewVokiRatingCreatedEvent(
    VokiRatingId RatingId,
    VokiId VokiId,
    AppUserId UserId,
    RatingValue RatingValue 
) : IDomainEvent;