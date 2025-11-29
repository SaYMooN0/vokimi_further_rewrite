using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;

namespace AlbumsService.Application.voki_albums.queries;

public sealed record GetVokiAlbumToViewQuery(
    VokiAlbumId AlbumId
) : IQuery<VokiAlbum>,
    IWithAuthCheckStep;

internal sealed class GetVokiAlbumToViewQueryHandler : IQueryHandler<GetVokiAlbumToViewQuery, VokiAlbum>
{
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IUserContext _userContext;

    public GetVokiAlbumToViewQueryHandler(IVokiAlbumsRepository vokiAlbumsRepository, IUserContext userContext) {
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<VokiAlbum>> Handle(GetVokiAlbumToViewQuery query, CancellationToken ct) {
        VokiAlbum? album = await _vokiAlbumsRepository.GetByIdAsNoTracking(query.AlbumId, ct);
        if (album is null) {
            return ErrFactory.NotFound.Common("Requested album not found");
        }

        if (album.OwnerId != _userContext.AuthenticatedUserId) {
            return ErrFactory.NoAccess("Cannot access this album because user is not owner");
        }

        return album;
    }
}