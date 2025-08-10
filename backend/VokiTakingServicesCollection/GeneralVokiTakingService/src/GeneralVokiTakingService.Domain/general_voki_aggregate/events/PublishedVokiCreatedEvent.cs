using VokimiStorageKeysLib;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.events;

public record class PublishedVokiCreatedEvent(
    VokiId VokiId,
    BaseStorageKey[] VokiContentKeys
) : IDomainEvent;