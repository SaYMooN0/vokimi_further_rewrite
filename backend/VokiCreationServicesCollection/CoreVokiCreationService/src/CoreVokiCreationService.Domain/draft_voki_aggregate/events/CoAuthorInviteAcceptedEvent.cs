namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public record class CoAuthorInviteAcceptedEvent(
    VokiId VokiId,
    AppUserId AppUserId
) : IDomainEvent;