using AlbumsService.Domain.app_user_aggregate;

namespace AlbumsService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user, CancellationToken ct);
    Task<AppUser?> GetById(AppUserId userId, CancellationToken ct);
    Task Update(AppUser user, CancellationToken ct);
    public Task<UserAutoAlbumsAppearance?> GetUsersAutoAlbumsAppearance(AppUserId userId, CancellationToken ct);
}