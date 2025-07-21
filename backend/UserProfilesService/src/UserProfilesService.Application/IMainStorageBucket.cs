using VokimiStorageKeysLib.users;

namespace UserProfilesService.Application;

public interface IMainStorageBucket
{
    public Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromDefaults(AppUserId userId);

    Task DeleteUserProfilePic(UserProfilePicKey key);
}