namespace VokiRatingsService.Domain.voki_ratings_snapshot.events;
public record VokiRatingsChangedCount(
    VokiId VokiId,
    uint NewRatingsCount
) : IDomainEvent;