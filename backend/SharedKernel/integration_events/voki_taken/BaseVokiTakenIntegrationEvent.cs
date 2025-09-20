using SharedKernel.common.vokis;

namespace SharedKernel.integration_events.voki_taken;

public abstract record class BaseVokiTakenIntegrationEvent(
    VokiId VokiId,
    AppUserId? VokiTakerId,
    VokiType VokiType,
    uint NewVokiTakingsCount
) : IIntegrationEvent;