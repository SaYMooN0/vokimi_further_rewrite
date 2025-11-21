using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate;
using SharedKernel.auth;

namespace AlbumsService.Application.app_users.commands;

public sealed record UpdateAutoAlbumsAppearanceCommand(
    UserAutoAlbumsAppearance NewAppearance
) : ICommand<UserAutoAlbumsAppearance>;

internal sealed class UpdateAutoAlbumsAppearanceCommandHandler :
    ICommandHandler<UpdateAutoAlbumsAppearanceCommand, UserAutoAlbumsAppearance>
{
    private readonly IUserContext _userContext;
    private readonly IAppUsersRepository _appUsersRepository;

    public UpdateAutoAlbumsAppearanceCommandHandler(IUserContext userContext, IAppUsersRepository appUsersRepository) {
        _userContext = userContext;
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<UserAutoAlbumsAppearance>> Handle(UpdateAutoAlbumsAppearanceCommand command, CancellationToken ct) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        AppUser? user = await _appUsersRepository.GetById(userId);
        if (user is null) {
            return ErrFactory.NotFound.User("Couldn't find user to update albums appearance");
        }

        user.UpdateAutoAlbumsAppearance(command.NewAppearance);
        await _appUsersRepository.Update(user);
        return user.AutoAlbumsAppearance;
    }
}