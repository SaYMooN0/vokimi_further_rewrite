namespace CoreVokiCreationService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiId> InitializedVokiIds { get; private set; }
    public ImmutableHashSet<VokiId> CoAuthoredVokiIds { get; private set; }
    //invite ids

    public AppUser(AppUserId id) {
        Id = id;
        InitializedVokiIds = [];
        CoAuthoredVokiIds = [];
    }

    public ErrOrNothing AddInitializedVoki(VokiId vokiId) {
        if (InitializedVokiIds.Contains(vokiId)) {
            return ErrFactory.Conflict("New Voki is already listed as initialized by this user");
        }

        InitializedVokiIds = InitializedVokiIds.Add(vokiId);
        return ErrOrNothing.Nothing;
    }
}