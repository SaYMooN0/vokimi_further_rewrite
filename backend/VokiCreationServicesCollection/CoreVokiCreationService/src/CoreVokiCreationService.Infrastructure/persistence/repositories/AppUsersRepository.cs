using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using InfrastructureShared.EfCore.query_extensions;

namespace CoreVokiCreationService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly CoreVokiCreationDbContext _db;

    public AppUsersRepository(CoreVokiCreationDbContext db) {
        _db = db;
    }

    public async Task<AppUser?> GetByIdForUpdate(AppUserId id, CancellationToken ct) =>
        await _db.AppUsers
            .ForUpdate()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: ct);

    public Task<AppUser[]> ListWithIdsForUpdate(IEnumerable<AppUserId> userIds, CancellationToken ct) =>
        _db.AppUsers
            .ForUpdate()
            .Where(u => userIds.Contains(u.Id))
            .ToArrayAsync(cancellationToken: ct);

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Update(AppUser user, CancellationToken ct) {
        _db.ThrowIfDetached(user);
        _db.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateRange(IEnumerable<AppUser> users, CancellationToken ct) {
        var materialized = users.ToList();

        _db.ThrowIfDetached(materialized);
        _db.UpdateRange(materialized);
        await _db.SaveChangesAsync(ct);
    }

    public Task<AppUser?> GetById(AppUserId userId, CancellationToken ct) =>
        _db.AppUsers.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: ct);
}