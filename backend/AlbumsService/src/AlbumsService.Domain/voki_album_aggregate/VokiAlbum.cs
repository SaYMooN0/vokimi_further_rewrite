namespace AlbumsService.Domain.voki_album_aggregate;

public class VokiAlbum : AggregateRoot<VokiAlbumId>
{
    // private VokiAlbum() { }
    public AppUserId OwnerId { get; }
    public AlbumName Name { get; private set; }
    public AlbumIcon Icon { get; private set; }
    public HexColor MainColor { get; private set; }
    public HexColor SecondColor { get; private set; }
    public ImmutableHashSet<VokiId> VokiIds { get; private set; }
    public DateTime CreationDate { get; }

    private VokiAlbum(
        VokiAlbumId id, AppUserId ownerId, AlbumName name, AlbumIcon icon,
        HexColor mainColor, HexColor secondColor, DateTime creationDate, ImmutableHashSet<VokiId> vokiIds
    ) {
        Id = id;
        OwnerId = ownerId;
        Name = name;
        Icon = icon;
        MainColor = mainColor;
        SecondColor = secondColor;
        CreationDate = creationDate;
        VokiIds = vokiIds;
    }

    public static VokiAlbum CreateNew(
        AppUserId ownerId, AlbumName name, AlbumIcon icon,
        HexColor mainColor, HexColor secondColor, DateTime creationDate
    ) {
        VokiAlbum album = new(VokiAlbumId.CreateNew(), ownerId, name, icon, mainColor, secondColor, creationDate, []);
        return album;
    }
}