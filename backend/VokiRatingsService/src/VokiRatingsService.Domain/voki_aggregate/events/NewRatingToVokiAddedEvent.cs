namespace VokiRatingsService.Domain.voki_aggregate.events;

public record NewRatingToVokiAddedEvent(
    VokiId VokiId,
    uint NewRatingsCount
) : IDomainEvent;