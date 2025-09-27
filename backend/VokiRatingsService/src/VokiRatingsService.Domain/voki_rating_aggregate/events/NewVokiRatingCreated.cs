namespace VokiRatingsService.Domain.voki_rating_aggregate.events;

public record NewVokiRatingCreated(
    VokiRatingId RatingId,
    VokiId VokiId,
    AppUserId UserId
) : IDomainEvent { }