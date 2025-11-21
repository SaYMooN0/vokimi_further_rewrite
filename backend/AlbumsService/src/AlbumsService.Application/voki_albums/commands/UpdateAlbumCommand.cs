using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.voki_album_aggregate;
using SharedKernel.auth;

namespace AlbumsService.Application.voki_albums.commands;

public sealed record UpdateAlbumCommand(
    VokiAlbumId AlbumId,
    AlbumName Name,
    AlbumIcon Icon,
    HexColor MainColor,
    HexColor SecondaryColor
) : ICommand<VokiAlbum>;

internal sealed class UpdateAlbumCommandHandler : ICommandHandler<UpdateAlbumCommand, VokiAlbum>
{
    private readonly IUserContext _userContext;
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;

    public UpdateAlbumCommandHandler(IUserContext userContext, IVokiAlbumsRepository vokiAlbumsRepository) {
        _userContext = userContext;
        _vokiAlbumsRepository = vokiAlbumsRepository;
    }

    public async Task<ErrOr<VokiAlbum>> Handle(UpdateAlbumCommand command, CancellationToken ct) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        VokiAlbum? album = await _vokiAlbumsRepository.GetById(command.AlbumId);
        if (album is null) {
            return ErrFactory.NotFound.Common("Could not update the album because it doesn't exist");
        }

        var res = album.Update(userId, command.Name, command.Icon, command.MainColor, command.SecondaryColor);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _vokiAlbumsRepository.Add(album);
        return album;
    }
}