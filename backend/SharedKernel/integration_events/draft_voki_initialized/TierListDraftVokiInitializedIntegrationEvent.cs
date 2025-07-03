using SharedKernel.common.vokis;

namespace SharedKernel.integration_events.draft_voki_initialized;

public record class TierListDraftVokiInitializedIntegrationEvent(
    VokiId VokiId,
    AppUserId PrimaryCreatorId,
    VokiName VokiName,
    DateTime CreationDate
) : IIntegrationEvent;