using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate;
using AlbumsService.Domain.voki_album_aggregate;
using SharedKernel.auth;

namespace AlbumsService.Application.voki_albums.queries;

public sealed record GetAllUserAlbumsPreviewQuery() : IQuery<GetAllUserAlbumsPreviewQueryResult>;

internal sealed class GetAllUserAlbumsPreviewQueryHandler :
    IQueryHandler<GetAllUserAlbumsPreviewQuery, GetAllUserAlbumsPreviewQueryResult>
{
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserContext _userContext;

    public GetAllUserAlbumsPreviewQueryHandler(
        IVokiAlbumsRepository vokiAlbumsRepository,
        IUserContext userContext,
        IAppUsersRepository appUsersRepository
    ) {
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _userContext = userContext;
        _appUsersRepository = appUsersRepository;
    }

    public async Task<ErrOr<GetAllUserAlbumsPreviewQueryResult>> Handle(
        GetAllUserAlbumsPreviewQuery query, CancellationToken ct
    ) {
        var userId = _userContext.AuthenticatedUserId;
        UserAutoAlbumsAppearance? albumsAppearance = await _appUsersRepository.GetUsersAutoAlbumsAppearance(userId, ct);
        if (albumsAppearance is null) {
            return ErrFactory.NotFound.User();
        }

        var albums = await _vokiAlbumsRepository.GetPreviewsForUserSortedAsNoTracking(_userContext.AuthenticatedUserId);
        return new GetAllUserAlbumsPreviewQueryResult(albumsAppearance, albums);
    }
}

public sealed record GetAllUserAlbumsPreviewQueryResult(
    UserAutoAlbumsAppearance AutoAlbumsAppearance,
    VokiAlbumPreviewDto[] Albums
);