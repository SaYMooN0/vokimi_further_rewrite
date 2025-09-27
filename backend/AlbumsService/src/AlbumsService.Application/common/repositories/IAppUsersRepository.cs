using AlbumsService.Domain.app_user_aggregate;

namespace AlbumsService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user);
    Task<AppUser?> GetById(AppUserId userId);
    Task Update(AppUser user);
}