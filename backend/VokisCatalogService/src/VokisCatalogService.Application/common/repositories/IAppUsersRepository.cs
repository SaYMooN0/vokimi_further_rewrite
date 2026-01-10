using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user, CancellationToken ct);
    Task<AppUser?> GetByIdForUpdate(AppUserId id, CancellationToken ct);
    Task Update(AppUser user, CancellationToken ct);
    Task UpdateRange(IEnumerable<AppUser> users, CancellationToken ct);
    Task<AppUser?> GetUserWithTakenVokisForUpdate(AppUserId userId, CancellationToken ct);
    Task<AppUser?> GetUserWithTakenVokis(AppUserId userId, CancellationToken ct);
}