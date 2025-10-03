using SharedKernel.common.app_users;
using UserProfilesService.Domain.app_user_aggregate;
using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task<AppUser?> GetById(AppUserId userId);
    Task Add(AppUser user);
    Task Update(AppUser user);
    Task<AppUser?> GetByIdAsNoTracking(AppUserId userId);

    public Task<Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)>> GetUserNamesWithProfilePics(
        IEnumerable<AppUserId> userIds,
        CancellationToken ct = default
    );
}