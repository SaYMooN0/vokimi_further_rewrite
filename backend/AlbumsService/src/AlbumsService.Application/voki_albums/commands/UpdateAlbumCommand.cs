using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.voki_albums.commands;

public sealed record UpdateAlbumCommand(
    VokiAlbumId AlbumId,
    AlbumName Name,
    AlbumIcon Icon,
    HexColor MainColor,
    HexColor SecondaryColor
) : ICommand<VokiAlbum>,
    IWithAuthCheckStep;

internal sealed class UpdateAlbumCommandHandler : ICommandHandler<UpdateAlbumCommand, VokiAlbum>
{
    private readonly IUserContext _userContext;
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;

    public UpdateAlbumCommandHandler(IUserContext userContext, IVokiAlbumsRepository vokiAlbumsRepository) {
        _userContext = userContext;
        _vokiAlbumsRepository = vokiAlbumsRepository;
    }

    public async Task<ErrOr<VokiAlbum>> Handle(UpdateAlbumCommand command, CancellationToken ct) {
        VokiAlbum? album = await _vokiAlbumsRepository.GetByIdForUpdate(command.AlbumId, ct);
        if (album is null) {
            return ErrFactory.NotFound.Common("Could not update the album because it doesn't exist");
        }

        var res = album.Update(new AuthenticatedUserCtx(_userContext.AuthenticatedUserId), command.Name, command.Icon, command.MainColor, command.SecondaryColor);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _vokiAlbumsRepository.Update(album, ct);
        return album;
    }
}