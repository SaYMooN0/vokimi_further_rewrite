namespace SharedKernel.integration_events;

public record class VokiRatedIntegrationEvent(
    VokiId VokiId,
    uint NewRatingsCount
) : IIntegrationEvent;