using AlbumsService.Domain.app_user_aggregate;
using AlbumsService.Domain.common.interfaces.repositories;

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
}