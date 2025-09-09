using VokiRatingsService.Domain.app_user_aggregate;
using VokiRatingsService.Domain.common.interfaces.repositories;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly VokiRatingsDbContext _db;

    public AppUsersRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user) {
        await _db.AppUsers.AddAsync(user);
        await _db.SaveChangesAsync();
    }
}