using SharedKernel.domain.events;
using VokimiStorageKeysLib.draft_voki_cover;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate.events;

public record class VokiCoverUpdatedEvent(
    VokiId VokiId,
    DraftVokiCoverKey NewCover
) : IDomainEvent;