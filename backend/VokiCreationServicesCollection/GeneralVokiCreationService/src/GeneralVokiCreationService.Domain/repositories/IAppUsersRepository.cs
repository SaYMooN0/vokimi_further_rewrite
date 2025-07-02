using GeneralVokiCreationService.Domain.app_user_aggregate;

namespace GeneralVokiCreationService.Domain.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user);
    Task<AppUser?> GetById(AppUserId id);
    Task Update(AppUser user);
}