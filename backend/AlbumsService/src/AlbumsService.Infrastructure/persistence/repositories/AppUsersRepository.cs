using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate;

namespace AlbumsService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly AlbumsDbContext _db;

    public AppUsersRepository(AlbumsDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user) {
        await _db.AppUsers.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task<AppUser?> GetById(AppUserId userId) => await _db.AppUsers.FindAsync(userId);

    public async Task Update(AppUser user) {
        _db.AppUsers.Update(user);
        await _db.SaveChangesAsync();
    }
}