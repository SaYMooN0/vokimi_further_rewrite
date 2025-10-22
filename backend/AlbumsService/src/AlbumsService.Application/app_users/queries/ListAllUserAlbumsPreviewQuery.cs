﻿using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate;
using SharedKernel.auth;

namespace AlbumsService.Application.app_users.queries;

public sealed record ListAllUserAlbumsPreviewQuery() : IQuery<ListAllUserAlbumsPreviewQueryResult>;

internal sealed class ListAllUserAlbumsPreviewQueryHandler :
    IQueryHandler<ListAllUserAlbumsPreviewQuery, ListAllUserAlbumsPreviewQueryResult>
{
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserContext _userContext;

    public ListAllUserAlbumsPreviewQueryHandler(
        IVokiAlbumsRepository vokiAlbumsRepository,
        IUserContext userContext,
        IAppUsersRepository appUsersRepository
    ) {
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _userContext = userContext;
        _appUsersRepository = appUsersRepository;
    }

    public async Task<ErrOr<ListAllUserAlbumsPreviewQueryResult>> Handle(
        ListAllUserAlbumsPreviewQuery query, CancellationToken ct
    ) {
        var userId = _userContext.AuthenticatedUserId;
        UserAutoAlbumsAppearance? albumsAppearance = await _appUsersRepository.GetUsersAutoAlbumsAppearance(userId, ct);
        if (albumsAppearance is null) {
            return ErrFactory.NotFound.User();
        }

        var albums = await _vokiAlbumsRepository.GetPreviewsForUserSortedAsNoTracking(_userContext.AuthenticatedUserId);
        return new ListAllUserAlbumsPreviewQueryResult(albumsAppearance, albums);
    }
}

public sealed record ListAllUserAlbumsPreviewQueryResult(
    UserAutoAlbumsAppearance AutoAlbumsAppearance,
    VokiAlbumPreviewDto[] Albums
);