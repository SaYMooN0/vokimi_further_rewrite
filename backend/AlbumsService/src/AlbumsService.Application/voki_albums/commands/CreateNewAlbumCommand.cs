using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate;
using AlbumsService.Domain.voki_album_aggregate;
using SharedKernel;
using SharedKernel.auth;

namespace AlbumsService.Application.voki_albums.commands;

public sealed record CreateNewAlbumCommand(
    AlbumName Name,
    AlbumIcon Icon,
    HexColor MainColor,
    HexColor SecondaryColor
) : ICommand<VokiAlbum>;

internal sealed class CreateNewAlbumCommandHandler :
    ICommandHandler<CreateNewAlbumCommand, VokiAlbum>
{
    private readonly IUserContext _userContext;
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IDateTimeProvider _dateTimeProvider;


    public CreateNewAlbumCommandHandler(
        IUserContext userContext, IVokiAlbumsRepository vokiAlbumsRepository,
        IAppUsersRepository appUsersRepository, IDateTimeProvider dateTimeProvider
    ) {
        _userContext = userContext;
        _vokiAlbumsRepository = vokiAlbumsRepository;
        _appUsersRepository = appUsersRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<VokiAlbum>> Handle(CreateNewAlbumCommand command, CancellationToken ct) {
        var userIdRes = _userContext.UserIdFromToken();
        if (userIdRes.IsErr()) {
            return ErrFactory.AuthRequired("To create albums you need to be logged in");
        }

        var userId = userIdRes.AsSuccess();
        AppUser? user = await _appUsersRepository.GetById(userId, ct);
        if (user is null) {
            return ErrFactory.NotFound.User();
        }

        VokiAlbum album = VokiAlbum.CreateNew(
            _userContext, command.Name, command.Icon,
            command.MainColor, command.SecondaryColor, _dateTimeProvider.UtcNow
        );
        var addingRes = user.AddAlbum(album.Id);
        if (addingRes.IsErr(out var err)) {
            return err;
        }

        await _appUsersRepository.Update(user, ct);
        await _vokiAlbumsRepository.Add(album, ct);

        return album;
    }
}