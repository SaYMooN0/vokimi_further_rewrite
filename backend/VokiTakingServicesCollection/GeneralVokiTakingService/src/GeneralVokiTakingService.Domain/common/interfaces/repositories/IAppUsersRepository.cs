using GeneralVokiTakingService.Domain.app_user_aggregate;

namespace GeneralVokiTakingService.Domain.common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user);
    Task<AppUser?> GetById(AppUserId id);
    Task Update(AppUser user);
}