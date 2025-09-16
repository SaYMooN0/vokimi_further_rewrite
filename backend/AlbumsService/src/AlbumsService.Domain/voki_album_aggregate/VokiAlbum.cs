namespace AlbumsService.Domain.voki_album_aggregate;

public class VokiAlbum : AggregateRoot<VokiAlbumId>
{
    private VokiAlbum() { }
    public AppUserId OwnerId { get; }
    public AlbumName Name { get; private set; }
    public AlbumIcon Icon { get; private set; }

    public HexColor MainColor { get; private set; }
    public HexColor SecondColor { get; private set; }
    public ImmutableHashSet<VokiId> VokiIds { get; private set; }

    private VokiAlbum(
        VokiAlbumId id, AppUserId ownerId, AlbumName name, AlbumIcon icon,
        HexColor mainColor, HexColor secondColor, ImmutableHashSet<VokiId> vokiIds
    ) {
        Id = id;
        OwnerId = ownerId;
        Name = name;
        Icon = icon;
        MainColor = mainColor;
        SecondColor = secondColor;
        VokiIds = vokiIds;
    }

    public VokiAlbum CreateNew(AppUserId ownerId, AlbumName name, AlbumIcon icon, HexColor mainColor, HexColor secondColor) {
        VokiAlbum album = new(VokiAlbumId.CreateNew(), ownerId, name, icon, mainColor, secondColor, []);
        return album;
    }
}