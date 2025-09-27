using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user);
    Task<AppUser?> GetById(AppUserId id);
    Task Update(AppUser user);
    Task UpdateRange(IEnumerable<AppUser> users);
}