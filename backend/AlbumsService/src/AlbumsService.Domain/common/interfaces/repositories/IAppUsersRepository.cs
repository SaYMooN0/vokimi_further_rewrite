using AlbumsService.Domain.app_user_aggregate;

namespace AlbumsService.Domain.common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user);
}