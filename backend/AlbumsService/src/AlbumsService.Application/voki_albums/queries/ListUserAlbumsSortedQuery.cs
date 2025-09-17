using AlbumsService.Domain.common.interfaces.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using SharedKernel.auth;

namespace AlbumsService.Application.voki_albums;

public sealed record ListUserAlbumsSortedQuery() : IQuery<VokiAlbum[]>;

internal sealed class ListUserAlbumsSortedQueryHandler : IQueryHandler<ListUserAlbumsSortedQuery, VokiAlbum[]>
{
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IUserContext _userContext;

    public ListUserAlbumsSortedQueryHandler(IVokiAlbumsRepository vokiAlbumsRepository, IUserContext userContext) {
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<VokiAlbum[]>> Handle(ListUserAlbumsSortedQuery query, CancellationToken ct) =>
        await _vokiAlbumsRepository.GetForUserSortedAsNoTracking(_userContext.AuthenticatedUserId);
}