using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public record NewDraftVokiInitializedEvent(
    VokiId VokiId,
    VokiType VokiType,
    VokiName VokiName,
    VokiCoverPath VokiCoverPath,    
    AppUserId PrimaryCreatorId,
    DateTime CreationDate
) : IDomainEvent;