using AuthService.Domain.common.interfaces;
using AuthService.Domain.rules;
using AuthService.Domain.unconfirmed_user_aggregate.events;

namespace AuthService.Domain.unconfirmed_user_aggregate;

public class UnconfirmedUser : AggregateRoot<UnconfirmedUserId>
{
    private UnconfirmedUser() { }
    public AppUserName UserName { get; private set; }
    public Email Email { get; }
    public string PasswordHash { get; private set; }
    private string ConfirmationCode { get; }

    private UnconfirmedUser(
        UnconfirmedUserId id,
        AppUserName userName,
        Email email,
        string passwordHash,
        string confirmationCode
    ) {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        ConfirmationCode = confirmationCode;
    }

    public static ErrOr<UnconfirmedUser> CreateNew(
        AppUserName username, Email email, string password, IPasswordHasher passwordHasher
    ) {
        if (PasswordRules.CheckForErr(password).IsErr(out var err)) {
            return err;
        }

        var user = new UnconfirmedUser(
            UnconfirmedUserId.CreateNew(),
            username,
            email,
            passwordHash: passwordHasher.Hash(password),
            confirmationCode: Guid.NewGuid().ToString()
        );
        user.AddDomainEvent(new UnconfirmedUserChangedEvent(
            user.Id, user.UserName, user.Email, user.ConfirmationCode)
        );
        return user;
    }

    public ErrOrNothing Override(AppUserName userName, string password, IPasswordHasher passwordHasher) {
        if (PasswordRules.CheckForErr(password).IsErr(out var err)) {
            return err;
        }

        this.PasswordHash = passwordHasher.Hash(password);
        this.UserName = userName;
        AddDomainEvent(new UnconfirmedUserChangedEvent(Id, UserName, Email, ConfirmationCode));
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing TryConfirm(string confirmationCode) {
        if (confirmationCode != this.ConfirmationCode) {
            return ErrFactory.Unspecified("Unable to confirm user. Incorrect confirmation data was provided");
        }

        return ErrOrNothing.Nothing;
    }
}