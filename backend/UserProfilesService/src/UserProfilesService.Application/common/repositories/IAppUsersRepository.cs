using SharedKernel.common.app_users;
using UserProfilesService.Domain.app_user_aggregate;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.profile_pics;

namespace UserProfilesService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task<AppUser?> GetById(AppUserId userId, CancellationToken ct);
    Task Add(AppUser user, CancellationToken ct);
    Task Update(AppUser user, CancellationToken ct);
    Task<AppUser?> GetByIdAsNoTracking(AppUserId userId, CancellationToken ct);

    public Task<UserPreviewDto[]> GetUserNamesWithProfilePics(
        IEnumerable<AppUserId> userIds, CancellationToken ct
    );
}

public record UserPreviewDto(
    AppUserId UserId,
    UserUniqueName UniqueName,
    UserDisplayName DisplayName,
    UserProfilePicKey ProfilePicKey
);