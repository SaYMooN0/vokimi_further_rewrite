using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Domain.common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task<AppUser?> GetById(AppUserId userId);
    Task Add(AppUser user);
    Task Update(AppUser user);
    Task<AppUser?> GetByIdAsNoTracking(AppUserId userId);
}