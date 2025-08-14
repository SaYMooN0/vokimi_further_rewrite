namespace VokisCatalogService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiId> InitializedVokiIds { get; private set; }
    public ImmutableHashSet<VokiId> CoAuthoredVokiIds { get; private set; }

    public AppUser(AppUserId id) {
        Id = id;
        InitializedVokiIds = [];
        CoAuthoredVokiIds = [];
    }

    public void AddInitializedVoki(VokiId vokiId) =>
        InitializedVokiIds = InitializedVokiIds.Add(vokiId);

    public void AddCoAuthoredVoki(VokiId vokiId) =>
        CoAuthoredVokiIds = CoAuthoredVokiIds.Add(vokiId);
}