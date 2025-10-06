using Microsoft.EntityFrameworkCore;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly VokisCatalogDbContext _db;

    public AppUsersRepository(VokisCatalogDbContext db) {
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

    public async Task UpdateRange(IEnumerable<AppUser> users) {
        _db.UpdateRange(users);
        await _db.SaveChangesAsync();
    }
    public Task<AppUser?> GetUserWithTakenVokis(AppUserId userId, CancellationToken ct) =>
        _db.AppUsers
            .Include(u => u.TakenVokis)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);
    public Task<AppUser?> GetUserWithTakenVokisAsNoTracking(AppUserId userId, CancellationToken ct) =>
        _db.AppUsers
            .AsNoTracking()
            .Include(u => u.TakenVokis)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);
}