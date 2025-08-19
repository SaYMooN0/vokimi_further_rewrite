

using SharedKernel.common.app_users;

namespace AuthService.Domain.unconfirmed_user_aggregate.events;

public record UnconfirmedUserChangedEvent(
    UnconfirmedUserId UserId,
    AppUserName Username,
    Email Email,
    string ConfirmationCode
) : IDomainEvent;