using VokiRatingsService.Domain.app_user_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user, CancellationToken ct = default);
    Task<AppUser?> GetById(AppUserId userId);
    Task<AppUser?> GetByIdAsNoTracking(AppUserId userId, CancellationToken ct);
    Task Update(AppUser appUser, CancellationToken ct = default);
}