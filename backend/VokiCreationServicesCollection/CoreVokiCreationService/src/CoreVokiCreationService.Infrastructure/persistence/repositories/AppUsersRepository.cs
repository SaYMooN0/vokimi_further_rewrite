using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using Microsoft.EntityFrameworkCore;

namespace CoreVokiCreationService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly CoreVokiCreationDbContext _db;

    public AppUsersRepository(CoreVokiCreationDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<AppUser?> GetById(AppUserId id, CancellationToken ct) =>
        await _db.AppUsers.FindAsync([id], cancellationToken: ct);

    public Task<AppUser[]> ListWithIds(IEnumerable<AppUserId> userIds, CancellationToken ct) =>
        _db.AppUsers
            .Where(u => userIds.Contains(u.Id)).ToArrayAsync(cancellationToken: ct);

    public async Task Update(AppUser user, CancellationToken ct) {
        _db.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public Task<AppUser?> GetByIdAsNoTracking(AppUserId userId, CancellationToken ct) =>
        _db.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: ct);

    public async Task UpdateRange(IEnumerable<AppUser> users, CancellationToken ct) {
        _db.UpdateRange(users);
        await _db.SaveChangesAsync(ct);
    }
}