using VokiTakingServicesLib.Domain.common;

namespace GeneralVokiTakingService.Domain.voki_taken_record_aggregate.events;

public record class VokiTakenRecordCreatedEvent(
    AppUserId? VokiTakerId,
    VokiTakenRecordId VokiTakenRecordId
) : IDomainEvent;