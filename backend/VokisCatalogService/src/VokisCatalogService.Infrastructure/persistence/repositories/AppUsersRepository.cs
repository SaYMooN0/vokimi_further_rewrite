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

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<AppUser?> GetById(AppUserId id, CancellationToken ct) =>
        await _db.AppUsers.FindAsync([id], cancellationToken:ct);

    public async Task Update(AppUser user, CancellationToken ct) {
        _db.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateRange(IEnumerable<AppUser> users, CancellationToken ct) {
        _db.UpdateRange(users);
        await _db.SaveChangesAsync(ct);
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