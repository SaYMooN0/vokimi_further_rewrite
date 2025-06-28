using AuthService.Domain.common;
using SharedKernel.common;
using SharedKernel.domain.events;

namespace AuthService.Domain.unconfirmed_user_aggregate.events;

public record UnconfirmedUserChangedEvent(
    UnconfirmedUserId UserId,
    AppUserName Username,
    Email Email,
    string ConfirmationCode
) : IDomainEvent;