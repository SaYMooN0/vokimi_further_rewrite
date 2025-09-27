using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Application.common;

public interface IMainStorageBucket
{
    public Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromDefaults(AppUserId userId);

}