using Microsoft.EntityFrameworkCore;
using SharedKernel.common.app_users;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;
using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Infrastructure.persistence.repositories;

public class AppUsersRepository : IAppUsersRepository
{
    private readonly UserProfilesDbContext _db;

    public AppUsersRepository(UserProfilesDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user) {
        await _db.AppUsers.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task<AppUser?> GetById(AppUserId id) =>
        await _db.AppUsers.FindAsync(id);

    public async Task Update(AppUser user) {
        _db.Update(user);
        await _db.SaveChangesAsync();
    }

    public Task<AppUser?> GetByIdAsNoTracking(AppUserId userId) => _db.AppUsers
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == userId);

    public Task<Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)>> GetUserNamesWithProfilePics(
        IEnumerable<AppUserId> userIds,
        CancellationToken ct = default
    ) {
        AppUserId[] userIdsArray = userIds as AppUserId[] ?? userIds.ToArray();

        if (!userIdsArray.Any()) {
            return Task.FromResult(new Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)>());
        }

        return _db.AppUsers
            .AsNoTracking()
            .Where(u => userIdsArray.Contains(u.Id))
            .Select(u => new { u.Id, u.UserName, u.ProfilePic })
            .ToDictionaryAsync(x => x.Id, x => (x.UserName, x.ProfilePic), ct);
    }
}