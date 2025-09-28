using Microsoft.EntityFrameworkCore;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.app_user_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly VokiRatingsDbContext _db;

    public AppUsersRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<AppUser?> GetById(AppUserId userId) => await _db.AppUsers.FindAsync(userId);

    public Task<AppUser?> GetByIdAsNoTracking(AppUserId userId, CancellationToken ct) => _db.AppUsers
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Id == userId, ct);

    public async Task Update(AppUser appUser, CancellationToken ct) {
        _db.AppUsers.Update(appUser);
        await _db.SaveChangesAsync(ct);
    }
}