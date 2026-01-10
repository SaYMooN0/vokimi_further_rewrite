using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.app_users.queries;

public sealed record ListAllUserAlbumsPreviewQuery() :
    IQuery<ListAllUserAlbumsPreviewQueryResult>,
    IWithAuthCheckStep;

internal sealed class ListAllUserAlbumsPreviewQueryHandler :
    IQueryHandler<ListAllUserAlbumsPreviewQuery, ListAllUserAlbumsPreviewQueryResult>
{
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ListAllUserAlbumsPreviewQueryHandler(
        IVokiAlbumsRepository vokiAlbumsRepository,
        IUserCtxProvider userCtxProvider,
        IAppUsersRepository appUsersRepository
    ) {
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _userCtxProvider = userCtxProvider;
        _appUsersRepository = appUsersRepository;
    }

    public async Task<ErrOr<ListAllUserAlbumsPreviewQueryResult>> Handle(
        ListAllUserAlbumsPreviewQuery query, CancellationToken ct
    ) {
        var userId = _userCtxProvider.AuthenticatedUserId;
        UserAutoAlbumsAppearance? albumsAppearance = await _appUsersRepository.GetUsersAutoAlbumsAppearance(userId, ct);
        if (albumsAppearance is null) {
            return ErrFactory.NotFound.User();
        }

        VokiAlbumPreviewDto[] albums = await _vokiAlbumsRepository.GetPreviewsForUserSorted(
            _userCtxProvider.AuthenticatedUserId, ct
        );
        return new ListAllUserAlbumsPreviewQueryResult(albumsAppearance, albums);
    }
}

public sealed record ListAllUserAlbumsPreviewQueryResult(
    UserAutoAlbumsAppearance AutoAlbumsAppearance,
    VokiAlbumPreviewDto[] Albums
);