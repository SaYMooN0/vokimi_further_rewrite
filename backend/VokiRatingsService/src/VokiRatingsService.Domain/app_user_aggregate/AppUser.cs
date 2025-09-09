namespace VokiRatingsService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiRatingId> RatingIds { get; private set; }

    public AppUser(AppUserId id) {
        Id = id;
        RatingIds = [];
    }
}