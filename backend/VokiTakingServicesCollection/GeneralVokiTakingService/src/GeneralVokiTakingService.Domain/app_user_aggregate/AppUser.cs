namespace GeneralVokiTakingService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public AppUser(AppUserId id) {
        Id = id;
    }
}