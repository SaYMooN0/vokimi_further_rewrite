namespace GeneralVokiTakingService.Domain.general_voki_aggregate.events;

public record class GeneralVokiTakenEvent(
    VokiId VokiId,
    AppUserId? VokiTakerId
) : IDomainEvent;