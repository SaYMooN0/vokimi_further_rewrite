namespace AlbumsService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiAlbumId> AlbumIds { get; private set; }

    public AppUser(AppUserId id) {
        Id = id;
        AlbumIds = [];
    }
}