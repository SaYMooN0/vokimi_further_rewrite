namespace VokiRatingsService.Domain.voki_rating_aggregate.events;

public record VokiRatingUpdatedEvent(
    VokiId VokiId
) : IDomainEvent;