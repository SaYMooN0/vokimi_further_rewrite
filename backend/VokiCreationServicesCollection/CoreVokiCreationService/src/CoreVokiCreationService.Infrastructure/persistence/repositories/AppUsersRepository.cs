using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.common.interfaces.repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreVokiCreationService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly CoreVokiCreationDbContext _db;

    public AppUsersRepository(CoreVokiCreationDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user) {
        _db.AppUsers.Add(user);
        await _db.SaveChangesAsync();
    }

    public async Task<AppUser?> GetById(AppUserId id) =>
        await _db.AppUsers.FindAsync(id);

    public async Task Update(AppUser user) {
        _db.Update(user);
        await _db.SaveChangesAsync();
    }

    public Task<AppUser?> GetByIdAsNoTracking(AppUserId userId) =>
        _db.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task UpdateRange(IEnumerable<AppUser> users) {
        _db.UpdateRange(users);
        await _db.SaveChangesAsync();
    }
}