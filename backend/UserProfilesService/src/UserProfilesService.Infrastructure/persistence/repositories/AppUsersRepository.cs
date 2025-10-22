using Microsoft.EntityFrameworkCore;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Infrastructure.persistence.repositories;

public class AppUsersRepository : IAppUsersRepository
{
    private readonly UserProfilesDbContext _db;

    public AppUsersRepository(UserProfilesDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<AppUser?> GetById(AppUserId id, CancellationToken ct) =>
        await _db.AppUsers.FindAsync([id], cancellationToken: ct);

    public async Task Update(AppUser user, CancellationToken ct) {
        _db.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public Task<AppUser?> GetByIdAsNoTracking(AppUserId userId, CancellationToken ct) => _db.AppUsers
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == userId, ct);

    public Task<UserPreviewDto[]> GetUserNamesWithProfilePics(
        IEnumerable<AppUserId> userIds,
        CancellationToken ct
    ) {
        AppUserId[] userIdsArray = userIds as AppUserId[] ?? userIds.ToArray();

        if (!userIdsArray.Any()) {
            return Task.FromResult(Array.Empty<UserPreviewDto>());
        }

        return _db.AppUsers
            .AsNoTracking()
            .Where(u => userIdsArray.Contains(u.Id))
            .Select(u => new UserPreviewDto(u.Id, u.UniqueName, u.DisplayName, u.ProfilePic))
            .ToArrayAsync(ct);
    }
}