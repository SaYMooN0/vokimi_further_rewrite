using SharedKernel.auth;
using UserProfilesService.Application.common;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;
using VokimiStorageKeysLib.concrete_keys.profile_pics;
using VokimiStorageKeysLib.temp_keys;

namespace UserProfilesService.Application.app_users.commands;

public sealed record SaveBasicProfileSetupCommand(
    string ProfilePic,
    UserDisplayName DisplayName,
    HashSet<Language> PreferredLanguages,
    ImmutableHashSet<VokiTagId> Tags
) : ICommand;

internal sealed class SaveBasicProfileSetupCommandHandler :
    ICommandHandler<SaveBasicProfileSetupCommand>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserContext _userContext;
    private readonly IMainStorageBucket _mainStorageBucket;

    public SaveBasicProfileSetupCommandHandler(
        IAppUsersRepository appUsersRepository,
        IUserContext userContext,
        IMainStorageBucket mainStorageBucket
    ) {
        _appUsersRepository = appUsersRepository;
        _userContext = userContext;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOrNothing> Handle(SaveBasicProfileSetupCommand command, CancellationToken ct) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        var savedKeyRes = await HandleProfilePicKey(userId, command.ProfilePic, ct);
        if (savedKeyRes.IsErr(out var err)) {
            return err;
        }

        UserProfilePicKey profilePicKey = savedKeyRes.AsSuccess();
        if (!profilePicKey.IsForUser(userId)) {
            return ErrFactory.Conflict(
                "Given profile pic key doesn't belong to this user",
                $" User id: {userId}, profile pic id: {profilePicKey.UserId}"
            );
        }

        AppUser? user = await _appUsersRepository.GetById(userId, ct);
        if (user is null) {
            return ErrFactory.NotFound.User(
                "User profile was not found", $"There is no user profile with id: {userId}"
            );
        }

        ErrOrNothing setupRes = user.ProcessBasicSetup(
            profilePicKey, 
            command.DisplayName,
            command.PreferredLanguages, command.Tags
            );
        if (savedKeyRes.IsErr(out err)) {
            return err;
        }

        await _appUsersRepository.Update(user, ct);
        return ErrOrNothing.Nothing;
    }

    private Task<ErrOr<UserProfilePicKey>> HandleProfilePicKey(AppUserId userId, string stringKey, CancellationToken ct) {
        if (TempImageKey.FromString(stringKey).IsSuccess(out var tempKey)) {
            return _mainStorageBucket.CopyUserProfilePicFromTemp(tempKey, userId, ct);
        }

        if (PresetProfilePicKey.CreateFromString(stringKey).IsSuccess(out var presetKey)) {
            return _mainStorageBucket.CopyUserProfilePicFromPresets(presetKey, userId, ct);
        }

        if (UserProfilePicKey.CreateFromString(stringKey).IsSuccess(out var savedKey)) {
            return Task.FromResult(ErrOr<UserProfilePicKey>.Success(savedKey));
        }

        return Task.FromResult(ErrOr<UserProfilePicKey>.Err(ErrFactory.Conflict(
            "Could not correctly handle provided profile picture",
            $"Key must be either temp, preset or saved. Provided key: '{stringKey}'"
        )));
    }
}