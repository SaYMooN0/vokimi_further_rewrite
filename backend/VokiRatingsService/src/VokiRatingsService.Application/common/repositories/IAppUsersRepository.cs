using VokiRatingsService.Domain.app_user_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user, CancellationToken ct);
    Task<AppUser?> GetByIdForUpdate(AppUserId userId, CancellationToken ct);
    Task<AppUser?> GetById(AppUserId userId, CancellationToken ct);
    Task Update(AppUser appUser, CancellationToken ct);
}