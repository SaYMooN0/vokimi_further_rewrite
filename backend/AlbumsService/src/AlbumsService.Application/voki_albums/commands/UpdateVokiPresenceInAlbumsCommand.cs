using AlbumsService.Application.common.repositories;
using AlbumsService.Application.voki_albums.queries;
using AlbumsService.Domain.voki_album_aggregate;
using SharedKernel.auth;

namespace AlbumsService.Application.voki_albums.commands;

public sealed record UpdateVokiPresenceInAlbumsCommand(
    VokiId VokiId,
    Dictionary<VokiAlbumId, bool> AlbumIdToIsChosen
) : ICommand<AlbumWithVokiPresenceDto[]>;

internal sealed class UpdateVokiPresenceInAlbumsCommandHandler :
    ICommandHandler<UpdateVokiPresenceInAlbumsCommand, AlbumWithVokiPresenceDto[]>
{
    private readonly IUserContext _userContext;
    private readonly IVokiAlbumsRepository _vokiAlbumsRepository;

    public UpdateVokiPresenceInAlbumsCommandHandler(IUserContext userContext, IVokiAlbumsRepository vokiAlbumsRepository) {
        _userContext = userContext;
        _vokiAlbumsRepository = vokiAlbumsRepository;
    }

    public async Task<ErrOr<AlbumWithVokiPresenceDto[]>> Handle(
        UpdateVokiPresenceInAlbumsCommand command, CancellationToken ct
    ) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        Dictionary<VokiAlbumId, VokiAlbum> albums = (await _vokiAlbumsRepository.ListAlbumsForUser(userId, ct))
            .ToDictionary(a => a.Id, a => a);
        List<VokiAlbum> changedAlbumsList = [];

        foreach (var (albumId, isChosen) in command.AlbumIdToIsChosen) {
            if (albums.TryGetValue(albumId, out var album)) {
                album.SetVokiPresenceTo(isChosen, command.VokiId);
                changedAlbumsList.Add(album);
            }
        }

        await _vokiAlbumsRepository.UpdateRange(changedAlbumsList, ct);
        return albums.Values
            .Select(a => AlbumWithVokiPresenceDto.FromAlbum(a, command.VokiId))
            .ToArray();
    }
}