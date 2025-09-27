using CoreVokiCreationService.Domain.app_user_aggregate;

namespace CoreVokiCreationService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user);
    Task<AppUser?> GetById(AppUserId id);
    Task Update(AppUser user);
    Task<AppUser?> GetByIdAsNoTracking(AppUserId userId);
    Task UpdateRange(IEnumerable<AppUser> users);
}