namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public record class CoAuthorInviteCreatedEvent(
    AppUserId InvitedUserId,
    VokiId VokiId
) : IDomainEvent;