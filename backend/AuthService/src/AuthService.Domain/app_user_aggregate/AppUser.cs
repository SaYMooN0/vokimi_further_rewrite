using AuthService.Domain.app_user_aggregate.events;
using AuthService.Domain.common.interfaces;
using SharedKernel;
using SharedKernel.common.app_users;

namespace AuthService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public Email Email { get; }
    public string PasswordHash { get; }
    public DateTime RegistrationDate { get; }
    public DateTime PasswordUpdateDate { get; private set; }

    private AppUser(AppUserId id, Email email, string passwordHash, DateTime registrationDate) {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        RegistrationDate = registrationDate;
        PasswordUpdateDate = registrationDate;
    }

    public static AppUser CreateNew(
        UnconfirmedUserId unconfirmedUserId, Email email, string passwordHash,
        AppUserName userName, IDateTimeProvider dateTimeProvider
    ) {
        var userId = new AppUserId(unconfirmedUserId.Value);
        AppUser user = new(userId, email, passwordHash, dateTimeProvider.UtcNow);
        user.AddDomainEvent(new NewAppUserCreatedEvent(user.Id, userName, user.RegistrationDate));
        return user;
    }

    public bool IsPasswordCorrect(IPasswordHasher passwordHasher, string password) {
        return passwordHasher.Verify(
            passwordToCheck: password,
            passwordHash: PasswordHash
        );
    }
}