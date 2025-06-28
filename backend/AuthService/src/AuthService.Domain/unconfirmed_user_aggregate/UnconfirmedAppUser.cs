

namespace AuthService.Domain.unconfirmed_user_aggregate;

public class UnconfirmedUser : AggregateRoot<UnconfirmedUserId>
{
    private UnconfirmedUser() { }
    public AppUserName Username { get; private set; }
    public Email Email { get; }
    public string PasswordHash { get; private set; }
    private string ConfirmationCode { get; }

}