using AuthService.Application.common.repositories;
using AuthService.Domain.app_user_aggregate;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private AuthDbContext _db;

    public AppUsersRepository(AuthDbContext db) {
        _db = db;
    }

    public Task<bool> AnyUserWithId(AppUserId appUserId, CancellationToken ct) => _db.AppUsers
        .AsNoTracking()
        .AnyAsync(u => u.Id == appUserId, cancellationToken: ct);

    public Task<bool> AnyUserWithEmail(Email email, CancellationToken ct) => _db.AppUsers
        .AsNoTracking()
        .AnyAsync(u => u.Email == email, cancellationToken: ct);

    public Task<AppUser?> GetByEmailAsNoTracking(Email email, CancellationToken ct) => _db.AppUsers
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Email == email, cancellationToken: ct);

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }
}