using SharedKernel.common.vokis;

namespace SharedKernel.integration_events.draft_voki_initialized;

public record class GeneralDraftVokiInitializedIntegrationEvent(
    VokiId VokiId,
    AppUserId PrimaryAuthorId,
    VokiName VokiName,
    DateTime CreationDate
) : IIntegrationEvent;