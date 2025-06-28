namespace AuthService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public Email Email { get; }
    public string PasswordHash { get; }

}