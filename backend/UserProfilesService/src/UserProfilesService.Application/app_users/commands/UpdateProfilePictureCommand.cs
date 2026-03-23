using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;
using UserProfilesService.Application.common;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;
using VokimiStorageKeysLib.concrete_keys.profile_pics;
using VokimiStorageKeysLib.temp_keys;

namespace UserProfilesService.Application.app_users.commands;

public sealed record UpdateProfilePictureCommand(
    string ProfilePic,
    ProfilePicShape Shape
) : ICommand,
    IWithAuthCheckStep;

internal sealed class UpdateProfilePictureCommandHandler : ICommandHandler<UpdateProfilePictureCommand>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IMainStorageBucket _mainStorageBucket;

    public UpdateProfilePictureCommandHandler(
        IAppUsersRepository appUsersRepository,
        IUserCtxProvider userCtxProvider,
        IMainStorageBucket mainStorageBucket
    ) {
        _appUsersRepository = appUsersRepository;
        _userCtxProvider = userCtxProvider;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOrNothing> Handle(UpdateProfilePictureCommand command, CancellationToken ct) {
        var userId = command.UserCtx(_userCtxProvider).UserId;
        var savedKeyRes = await HandleProfilePicKey(userId, command.ProfilePic, ct);
        if (savedKeyRes.IsErr(out var err)) {
            return err;
        }

        UserProfilePicKey profilePicKey = savedKeyRes.AsSuccess();

        AppUser? user = await _appUsersRepository.GetByIdForUpdate(userId, ct);
        if (user is null) {
            return ErrFactory.NotFound.User(
                "User profile was not found", $"There is no user profile with id: {userId}"
            );
        }

        ErrOrNothing updateRes = user.UpdateProfilePic(new UserProfilePic(profilePicKey, command.Shape));
        if (updateRes.IsErr(out err)) {
            return err;
        }

        await _appUsersRepository.Update(user, ct);
        return ErrOrNothing.Nothing;
    }

    private Task<ErrOr<UserProfilePicKey>> HandleProfilePicKey(
        AppUserId userId, string stringKey, CancellationToken ct
    ) {
        if (TempImageKey.FromString(stringKey).IsSuccess(out var tempKey)) {
            return _mainStorageBucket.CopyUserProfilePicFromTemp(tempKey, userId, ct);
        }

        if (UserProfilePicKey.CreateFromString(stringKey).IsSuccess(out var savedKey)) {
            return Task.FromResult(ErrOr<UserProfilePicKey>.Success(savedKey));
        }

        return Task.FromResult(ErrOr<UserProfilePicKey>.Err(ErrFactory.Conflict(
            "Could not correctly handle provided profile picture",
            $"Key must be either temp or saved. Provided key: '{stringKey}'"
        )));
    }
}