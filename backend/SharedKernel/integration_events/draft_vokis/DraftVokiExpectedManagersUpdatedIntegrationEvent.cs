namespace SharedKernel.integration_events.draft_vokis;

public record class DraftVokiExpectedManagersUpdatedIntegrationEvent(
    string msg
) : IIntegrationEvent;

// VokiId VokiId,
// VokiType VokiType,
// AppUserId[] ExpectedManagers,