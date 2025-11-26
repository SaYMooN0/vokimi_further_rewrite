using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.auth;

namespace AlbumsService.Application.voki_albums.commands;

public sealed record CopyVokisFromAlbumsToAlbumCommand(
    VokiAlbumId AlbumId,
    ImmutableHashSet<VokiAlbumId> AlbumIdsToCopyFrom
) :
    ICommand<int>,
    IWithBasicValidationStep
{
    public ErrOrNothing Validate() {
        if (AlbumIdsToCopyFrom.Count == 0) {
            return ErrFactory.NoValue.Common("Albums to copy from are not specified");
        }

        if (AlbumIdsToCopyFrom.Count > VokiAlbum.MaxAlbumsToCopyFrom) {
            return ErrFactory.LimitExceeded(
                $"Too many source albums specified. Maximum allowed: {VokiAlbum.MaxAlbumsToCopyFrom}",
                $"Provided: {AlbumIdsToCopyFrom.Count}"
            );
        }

        return ErrOrNothing.Nothing;
    }
}

internal sealed class CopyVokisFromAlbumsToAlbumCommandHandler : ICommandHandler<CopyVokisFromAlbumsToAlbumCommand, int>
{
    private readonly IUserContext _userContext;
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;

    public CopyVokisFromAlbumsToAlbumCommandHandler(IUserContext userContext,
        IVokiAlbumsRepository vokiAlbumsRepository) {
        _userContext = userContext;
        _vokiAlbumsRepository = vokiAlbumsRepository;
    }

    public async Task<ErrOr<int>> Handle(CopyVokisFromAlbumsToAlbumCommand command, CancellationToken ct) {
        VokiAlbum? album = await _vokiAlbumsRepository.GetById(command.AlbumId, ct);
        if (album is null) {
            return ErrFactory.NotFound.Common("Could not copy vokis because destination does not exist");
        }

        var albumsToCopyFrom = await _vokiAlbumsRepository.ListByIdsAsNoTracking(command.AlbumIdsToCopyFrom, ct);
        ErrOr<int> copyRes = album.CopyVokisFromAlbums(_userContext, albumsToCopyFrom);
        if (copyRes.IsErr(out var err)) {
            return err;
        }

        await _vokiAlbumsRepository.Update(album, ct);
        return copyRes.AsSuccess();
    }
}