using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.voki_albums.queries;

public sealed record ListAlbumsDataForVokiQuery(
    VokiId VokiId
) : IQuery<AlbumWithVokiPresenceDto[]>,
    IWithAuthCheckStep;

internal sealed class ListAlbumsDataForVokiQueryHandler :
    IQueryHandler<ListAlbumsDataForVokiQuery, AlbumWithVokiPresenceDto[]>
{
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IUserContext _userContext;

    public ListAlbumsDataForVokiQueryHandler(IVokiAlbumsRepository vokiAlbumsRepository, IUserContext userContext) {
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<AlbumWithVokiPresenceDto[]>> Handle(
        ListAlbumsDataForVokiQuery query, CancellationToken ct
    ) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        VokiAlbum[] albums = await _vokiAlbumsRepository.ListAlbumsForUser(userId, ct);
        return albums
            .Select(a => AlbumWithVokiPresenceDto.FromAlbum(a, query.VokiId))
            .ToArray();
    }
}

public record AlbumWithVokiPresenceDto(
    VokiAlbumId Id,
    AlbumName Name,
    AlbumIcon Icon,
    HexColor MainColor,
    HexColor SecondaryColor,
    DateTime CreationDate,
    bool IsVokiInAlbum
)
{
    public static AlbumWithVokiPresenceDto FromAlbum(VokiAlbum album, VokiId vokiId) => new(
        album.Id,
        album.Name,
        album.Icon,
        album.MainColor,
        album.SecondaryColor,
        album.CreationDate,
        album.HasVoki(vokiId)
    );
}