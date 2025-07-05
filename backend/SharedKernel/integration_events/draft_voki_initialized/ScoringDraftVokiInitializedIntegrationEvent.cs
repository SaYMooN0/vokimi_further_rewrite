using SharedKernel.common.vokis;

namespace SharedKernel.integration_events.draft_voki_initialized;

public record class ScoringDraftVokiInitializedIntegrationEvent(
    VokiId VokiId,
    AppUserId PrimaryAuthorId,
    string VokiName,
    VokiCoverPath CoverPath,
    DateTime CreationDate
) : IIntegrationEvent;