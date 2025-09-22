using GeneralVokiTakingService.Domain.app_user_aggregate;

namespace GeneralVokiTakingService.Domain.common.interfaces.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user, CancellationToken ct = default);
    Task<AppUser?> GetById(AppUserId id, CancellationToken ct);
    Task Update(AppUser user, CancellationToken ct);
    Task<AppUser?> GetByIdAsNoTracking(AppUserId userId, CancellationToken ct);
}