using VokiCommentsService.Domain.app_user_aggregate;

namespace VokiCommentsService.Domain.common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user);
}