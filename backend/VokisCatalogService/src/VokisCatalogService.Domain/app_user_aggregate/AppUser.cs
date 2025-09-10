namespace VokisCatalogService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiId> InitializedVokiIds { get; private set; }
    public ImmutableHashSet<VokiId> CoAuthoredVokiIds { get; private set; }
    public ImmutableHashSet<VokiId> TakenVokiIds { get; private set; }

    public AppUser(AppUserId id) {
        Id = id;
        InitializedVokiIds = [];
        CoAuthoredVokiIds = [];
        TakenVokiIds = [];
    }

    public void AddInitializedVoki(VokiId vokiId) =>
        InitializedVokiIds = InitializedVokiIds.Add(vokiId);

    public void AddCoAuthoredVoki(VokiId vokiId) =>
        CoAuthoredVokiIds = CoAuthoredVokiIds.Add(vokiId); 
    public void AddTakenVoki(VokiId vokiId) =>
        TakenVokiIds = TakenVokiIds.Add(vokiId);
}