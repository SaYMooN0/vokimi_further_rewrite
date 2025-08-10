using SharedKernel.domain.events;
using VokimiStorageKeysLib.voki_cover;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate.events;

public record class VokiCoverUpdatedEvent(
    VokiId VokiId,
    VokiCoverKey OldCover,
    VokiCoverKey NewCover
) : IDomainEvent;