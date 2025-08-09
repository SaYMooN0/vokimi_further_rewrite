namespace VokisCatalogService.Domain.voki_aggregate.events;

public record class PublishedVokiCreatedEvent(
    VokiId VokiId
) : IDomainEvent;