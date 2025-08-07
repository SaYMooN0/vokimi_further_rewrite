namespace VokisCatalogService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiId> InitializedVokiIds { get; private set; }
    private ImmutableHashSet<VokiId> CoAuthoredVokiIds { get; init; }

    public AppUser(AppUserId id) {
        Id = id;
        InitializedVokiIds = [];
        CoAuthoredVokiIds = [];
    }
}