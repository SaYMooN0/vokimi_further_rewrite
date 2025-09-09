using Microsoft.EntityFrameworkCore;
using UserProfilesService.Domain.app_user_aggregate;
using UserProfilesService.Domain.common.interfaces.repositories;

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
}