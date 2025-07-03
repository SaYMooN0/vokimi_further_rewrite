using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public record class CoAuthorInviteCreatedEvent(
    AppUserId InvitedUserId,
    VokiType VokiId
) : IDomainEvent;