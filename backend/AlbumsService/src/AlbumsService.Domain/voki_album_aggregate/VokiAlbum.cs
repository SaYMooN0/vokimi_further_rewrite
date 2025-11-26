using SharedKernel.auth;

namespace AlbumsService.Domain.voki_album_aggregate;

public class VokiAlbum : AggregateRoot<VokiAlbumId>
{
    private VokiAlbum() { }
    public AppUserId OwnerId { get; }
    public AlbumName Name { get; private set; }
    public AlbumIcon Icon { get; private set; }
    public HexColor MainColor { get; private set; }
    public HexColor SecondaryColor { get; private set; }
    public ImmutableHashSet<VokiId> VokiIds { get; private set; }
    public DateTime CreationDate { get; }

    private VokiAlbum(
        VokiAlbumId id, AppUserId ownerId, AlbumName name, AlbumIcon icon,
        HexColor mainColor, HexColor secondaryColor, DateTime creationDate, ImmutableHashSet<VokiId> vokiIds
    ) {
        Id = id;
        OwnerId = ownerId;
        Name = name;
        Icon = icon;
        MainColor = mainColor;
        SecondaryColor = secondaryColor;
        CreationDate = creationDate;
        VokiIds = vokiIds;
    }

    public static VokiAlbum CreateNew(
        IUserContext userContext, AlbumName name, AlbumIcon icon,
        HexColor mainColor, HexColor secondaryColor, DateTime creationDate
    ) {
        VokiAlbum album = new(
            VokiAlbumId.CreateNew(), userContext.AuthenticatedUserId,
            name, icon, mainColor, secondaryColor, creationDate,
            vokiIds: []
        );
        return album;
    }

    public bool HasVoki(VokiId vokiId) => VokiIds.Contains(vokiId);

    public ErrOrNothing SetVokiPresenceTo(IUserContext userContext, bool presence, VokiId voki) {
        if (userContext.AuthenticatedUserId != OwnerId) {
            return ErrFactory.NoAccess();
        }

        if (presence) {
            VokiIds = VokiIds.Add(voki);
        }
        else {
            VokiIds = VokiIds.Remove(voki);
        }

        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing Update(
        IUserContext userContext, AlbumName name, AlbumIcon icon,
        HexColor mainColor, HexColor secondaryColor
    ) {
        if (userContext.AuthenticatedUserId != OwnerId) {
            return ErrFactory.NoAccess("Could not update the album because user is not owner");
        }

        Name = name;
        Icon = icon;
        MainColor = mainColor;
        SecondaryColor = secondaryColor;
        return ErrOrNothing.Nothing;
    }

    public const int MaxAlbumsToCopyFrom = 120;

    public ErrOr<int> CopyVokisFromAlbums(IUserContext userContext, VokiAlbum[] albumsToCopyFrom) {
        if (albumsToCopyFrom.Length > MaxAlbumsToCopyFrom) {
            return ErrFactory.LimitExceeded(
                $"Too many source albums specified. Maximum allowed: {MaxAlbumsToCopyFrom}",
                $"Provided: {albumsToCopyFrom.Length}"
            );
        }

        if (albumsToCopyFrom.Length == 0) {
            return ErrFactory.NoValue.Common("Albums to copy from are not specified");
        }

        HashSet<VokiId> albumsToAdd = new(capacity: 16);
        foreach (var a in albumsToCopyFrom) {
            if (a.OwnerId != userContext.AuthenticatedUserId) {
                return ErrFactory.NoAccess(
                    "Users are not allowed to copy Vokis from albums they do not have access to"
                );
            }

            albumsToAdd.UnionWith(a.VokiIds);
        }

        int oldCount = VokiIds.Count;
        VokiIds = VokiIds.Union(albumsToAdd);
        return VokiIds.Count - oldCount;
    }
}