using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task<AppUser?> GetById(AppUserId userId);
    Task Add(AppUser user);
    Task Update(AppUser user);
    Task<AppUser?> GetByIdAsNoTracking(AppUserId userId);
}