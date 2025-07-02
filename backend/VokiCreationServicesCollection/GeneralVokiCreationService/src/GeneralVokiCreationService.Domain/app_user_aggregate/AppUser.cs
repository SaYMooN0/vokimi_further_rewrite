using System.Collections.Immutable;

namespace GeneralVokiCreationService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    private ImmutableHashSet<VokiId> InitializedVokiIds { get; init; }
    private ImmutableHashSet<VokiId> CoAuthoredVokiIds { get; init; }

    public AppUser(AppUserId id) {
        Id = id;
        InitializedVokiIds = [];
        CoAuthoredVokiIds = [];
    }
}