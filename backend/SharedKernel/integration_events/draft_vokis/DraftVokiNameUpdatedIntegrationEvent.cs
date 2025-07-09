using SharedKernel.common.vokis;

namespace SharedKernel.integration_events.draft_vokis;

public record class DraftVokiNameUpdatedIntegrationEvent(
    VokiId VokiId,
    VokiName NewVokiName
) : IIntegrationEvent;