using VokiRatingsService.Domain.app_user_aggregate;

namespace VokiRatingsService.Domain.common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user);
}