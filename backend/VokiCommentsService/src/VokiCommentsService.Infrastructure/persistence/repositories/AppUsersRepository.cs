using VokiCommentsService.Domain.app_user_aggregate;
using VokiCommentsService.Domain.common.interfaces.repositories;

namespace VokiCommentsService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly VokiCommentsDbContext _db;

    public AppUsersRepository(VokiCommentsDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user) {
        await _db.AddAsync(user);
        await _db.SaveChangesAsync();
    }
}