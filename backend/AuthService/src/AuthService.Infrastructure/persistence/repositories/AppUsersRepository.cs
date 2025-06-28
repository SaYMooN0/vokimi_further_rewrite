using AuthService.Domain.app_user_aggregate;
using AuthService.Domain.common.interfaces.repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private AuthDbContext _db;

    public AppUsersRepository(AuthDbContext db) {
        _db = db;
    }

    public Task<bool> AnyUserWithId(AppUserId appUserId) => _db.AppUsers
        .AsNoTracking()
        .AnyAsync(u => u.Id == appUserId);

    public Task<bool> AnyUserWithEmail(Email email) => _db.AppUsers
        .AsNoTracking()
        .AnyAsync(u => u.Email == email);

    public Task<AppUser?> GetByEmailAsNoTracking(Email email) => _db.AppUsers
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Email == email);

    public Task Add(AppUser user) {
        _db.AppUsers.Add(user);
        return _db.SaveChangesAsync();
    }

    public async Task<AppUser?> GetById(AppUserId userId) => await _db.AppUsers.FindAsync(userId);

    public async Task Update(AppUser user) {
        _db.AppUsers.Update(user);
        await _db.SaveChangesAsync();
    }
}