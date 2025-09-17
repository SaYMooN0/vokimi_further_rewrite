using AlbumsService.Domain.app_user_aggregate.events;

namespace AlbumsService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }
    public ImmutableHashSet<VokiAlbumId> AlbumIds { get; private set; }

    public AppUser(AppUserId id) {
        Id = id;
        AlbumIds = [];
    }

    public void AddAlbum(VokiAlbumId id) {
        AlbumIds = AlbumIds.Add(id);
    }

    public void DeleteAlbum(VokiAlbumId id) {
        if (AlbumIds.Contains(id)) {
            AlbumIds = AlbumIds.Remove(id);
            AddDomainEvent(new VokiAlbumDeletedDomainEvent(id));
        }
    }
}