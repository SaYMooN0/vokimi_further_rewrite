using AuthService.Application.common.repositories;
using AuthService.Domain.unconfirmed_user_aggregate;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.persistence.repositories;

internal class UnconfirmedUsersRepository : IUnconfirmedUsersRepository
{
    private AuthDbContext _db;

    public UnconfirmedUsersRepository(AuthDbContext db) {
        _db = db;
    }

    public Task<UnconfirmedUser?> GetByEmail(Email email, CancellationToken ct) =>
        _db.UnconfirmedUsers.FirstOrDefaultAsync(u => u.Email == email, cancellationToken: ct);

    public async Task Add(UnconfirmedUser unconfirmedUser, CancellationToken ct) {
        await _db.UnconfirmedUsers.AddAsync(unconfirmedUser, ct);
        await _db.SaveChangesAsync(ct);
    }

    public Task Update(UnconfirmedUser unconfirmedUser, CancellationToken ct) {
        _db.UnconfirmedUsers.Update(unconfirmedUser);
        return _db.SaveChangesAsync(ct);
    }

    public async Task<UnconfirmedUser?> GetById(UnconfirmedUserId userId, CancellationToken ct) =>
        await _db.UnconfirmedUsers.FindAsync([userId], cancellationToken: ct);

    public Task Delete(UnconfirmedUser unconfirmedUser, CancellationToken ct) {
        _db.UnconfirmedUsers.Remove(unconfirmedUser);
        return _db.SaveChangesAsync(ct);
    }

    public async Task<int> DeleteAllExpiredUsers(DateTime utcNow, CancellationToken ct) {
        return await _db.UnconfirmedUsers
            .Where(u =>  u.ExpiresAt <= utcNow)
            .ExecuteDeleteAsync(ct);
    }
}