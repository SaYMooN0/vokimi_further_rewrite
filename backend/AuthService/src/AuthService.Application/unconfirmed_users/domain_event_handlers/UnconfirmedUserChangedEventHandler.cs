using AuthService.Application.abstractions;
using AuthService.Domain.unconfirmed_user_aggregate.events;


namespace AuthService.Application.unconfirmed_users.domain_event_handlers;

public class UnconfirmedUserChangedEventHandler : IDomainEventHandler<UnconfirmedUserChangedEvent>
{
    private readonly IEmailService _emailService;

    public UnconfirmedUserChangedEventHandler(IEmailService emailService) {
        _emailService = emailService;
    }

    public async Task Handle(UnconfirmedUserChangedEvent e, CancellationToken ct) {
        // var sendingErr = await _emailService.SendRegistrationConfirmationLink(
        //     e.Email, e.Username, e.UserId, e.ConfirmationCode
        // );
        //
        // UnexpectedBehaviourException.ThrowIfErr(
        //     sendingErr, "Unable to send email confirmation link. Please try again later"
        // );
    }
}