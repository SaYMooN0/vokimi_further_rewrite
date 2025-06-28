using AuthService.Domain.common;
using SharedKernel.common;

namespace AuthService.Application.abstractions;

public interface IEmailService
{
    Task<ErrOrNothing> SendRegistrationConfirmationLink(
        Email email,
        AppUserName username,
        UnconfirmedUserId userId,
        string confirmationCode
    );
}