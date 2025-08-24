using SharedKernel.common.app_users;

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