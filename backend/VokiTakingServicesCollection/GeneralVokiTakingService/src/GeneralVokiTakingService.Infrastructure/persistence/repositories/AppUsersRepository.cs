using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Domain.app_user_aggregate;
using Microsoft.EntityFrameworkCore;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public AppUsersRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<AppUser?> GetById(AppUserId id, CancellationToken ct) =>
        await _db.AppUsers.FindAsync(keyValues: [id], cancellationToken: ct);

    public async Task Update(AppUser user, CancellationToken ct) {
        _db.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<AppUser?> GetByIdAsNoTracking(AppUserId userId, CancellationToken ct) =>
        await _db.AppUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, ct);
}