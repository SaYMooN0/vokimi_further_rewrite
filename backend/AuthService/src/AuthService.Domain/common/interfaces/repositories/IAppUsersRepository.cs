using AuthService.Domain.app_user_aggregate;

namespace AuthService.Domain.common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task<bool> AnyUserWithId(AppUserId appUserId);
    Task Add(AppUser user);
    
    Task<bool> AnyUserWithEmail(Email email);
    Task<AppUser?> GetByEmailAsNoTracking(Email email);
}