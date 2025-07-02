using GeneralVokiCreationService.Domain.app_user_aggregate;
using GeneralVokiCreationService.Domain.repositories;

namespace GeneralVokiCreationService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly GeneralVokiCreationDbContext _db;

    public AppUsersRepository(GeneralVokiCreationDbContext db) {
        _db = db;
    }

    public Task Add(AppUser user) {
        _db.AppUsers.Add(user);
        return _db.SaveChangesAsync();
    }

    public async Task<AppUser?> GetById(AppUserId id) =>
        await _db.AppUsers.FindAsync(id);

    public async Task Update(AppUser user) {
        _db.Update(user);
        await _db.SaveChangesAsync();
    }
}