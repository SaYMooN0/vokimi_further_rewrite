using System.Collections.Immutable;

namespace CoreVokiCreationService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    private ImmutableHashSet<VokiId> InitializedVokiIds { get; init; }
    private ImmutableHashSet<VokiId> CoAuthoredVokiIds { get; init; }
    //invite ids

    public AppUser(AppUserId id) {
        Id = id;
        InitializedVokiIds = [];
        CoAuthoredVokiIds = [];
    }
}