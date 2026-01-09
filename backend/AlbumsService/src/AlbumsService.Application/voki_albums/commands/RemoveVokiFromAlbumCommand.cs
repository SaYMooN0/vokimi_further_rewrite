using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.voki_albums.commands;

public sealed record RemoveVokiFromAlbumCommand(
    VokiAlbumId AlbumId,
    VokiId VokiId
) : ICommand,
    IWithAuthCheckStep;

internal sealed class RemoveVokiFromAlbumCommandHandler : ICommandHandler<RemoveVokiFromAlbumCommand>
{
    private readonly IUserContext _userContext;
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;

    public RemoveVokiFromAlbumCommandHandler(IUserContext userContext, IVokiAlbumsRepository vokiAlbumsRepository) {
        _userContext = userContext;
        _vokiAlbumsRepository = vokiAlbumsRepository;
    }

    public async Task<ErrOrNothing> Handle(RemoveVokiFromAlbumCommand command, CancellationToken ct) {
        VokiAlbum? album = await _vokiAlbumsRepository.GetByIdForUpdate(command.AlbumId, ct);
        if (album is null) {
            return ErrFactory.NotFound.Common("Could not update the album because it doesn't exist");
        }

        var res =
            album.RemoveVoki(new AuthenticatedUserCtx(_userContext.AuthenticatedUserId), command.VokiId);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _vokiAlbumsRepository.Update(album, ct);
        return ErrOrNothing.Nothing;
    }
}