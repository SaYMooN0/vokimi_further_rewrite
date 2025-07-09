using SharedKernel.common.vokis;
using VokimiStorageKeysLib.draft_voki_cover;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public record NewDraftVokiInitializedEvent(
    VokiId VokiId,
    VokiType Type,
    VokiName Name,
    DraftVokiCoverKey Cover,    
    AppUserId PrimaryAuthorId,
    DateTime CreationDate
) : IDomainEvent;