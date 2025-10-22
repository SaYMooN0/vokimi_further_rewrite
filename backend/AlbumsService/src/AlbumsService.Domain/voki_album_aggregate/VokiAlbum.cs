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
        AppUserId ownerId, AlbumName name, AlbumIcon icon,
        HexColor mainColor, HexColor secondaryColor, DateTime creationDate
    ) {
        VokiAlbum album = new(
            VokiAlbumId.CreateNew(), ownerId,
            name, icon, mainColor, secondaryColor, creationDate,
            vokiIds: []
        );
        return album;
    }

    public bool HasVoki(VokiId vokiId) => VokiIds.Contains(vokiId);

    public void SetVokiPresenceTo(bool presence, VokiId voki) {
        if (presence) {
            VokiIds = VokiIds.Add(voki);
        }
        else {
            VokiIds = VokiIds.Remove(voki);
        }
    }
}