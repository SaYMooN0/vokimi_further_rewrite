using AlbumsService.Domain.app_user_aggregate;

namespace AlbumsService.Domain.common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user);
    Task<AppUser?> GetById(AppUserId userId);
    Task Update(AppUser user);
}