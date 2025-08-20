using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Application;

public interface IMainStorageBucket
{
    public Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromDefaults(AppUserId userId);

}