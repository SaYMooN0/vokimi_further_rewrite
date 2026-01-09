using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate;
using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.app_users.commands;

public sealed record DeleteAlbumCommand(
    VokiAlbumId AlbumId
) : ICommand,
    IWithAuthCheckStep;

internal sealed class DeleteAlbumCommandHandler : ICommandHandler<DeleteAlbumCommand>
{
    private readonly IUserContext _userContext;
    private readonly IAppUsersRepository _appUsersRepository;

    public DeleteAlbumCommandHandler(IUserContext userContext, IAppUsersRepository appUsersRepository) {
        _userContext = userContext;
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOrNothing> Handle(DeleteAlbumCommand command, CancellationToken ct) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        AppUser? user = await _appUsersRepository.GetByIdForUpdate(userId,ct);
        if (user is null) {
            return ErrFactory.NotFound.User("Couldn't delete album because the owner was not found");
        }
        user.DeleteAlbum(command.AlbumId);
        await _appUsersRepository.Update(user, ct); 
        return ErrOrNothing.Nothing;
    }
}