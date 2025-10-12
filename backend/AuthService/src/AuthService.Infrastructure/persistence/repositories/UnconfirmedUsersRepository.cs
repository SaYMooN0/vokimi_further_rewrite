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

    public Task<UnconfirmedUser?> GetByEmail(Email email) =>
        _db.UnconfirmedUsers.FirstOrDefaultAsync(u => u.Email == email);

    public async Task Add(UnconfirmedUser unconfirmedUser) {
        await _db.UnconfirmedUsers.AddAsync(unconfirmedUser);
        await _db.SaveChangesAsync();
    }

    public Task Update(UnconfirmedUser unconfirmedUser) {
        _db.UnconfirmedUsers.Update(unconfirmedUser);
        return _db.SaveChangesAsync();
    }

    public async Task<UnconfirmedUser?> GetById(UnconfirmedUserId userId) =>
        await _db.UnconfirmedUsers.FindAsync(userId);

    public Task Delete(UnconfirmedUser unconfirmedUser) {
        _db.UnconfirmedUsers.Remove(unconfirmedUser);
        return _db.SaveChangesAsync();
    }

    public async Task<int> DeleteAllExpiredUsers(DateTime utcNow, CancellationToken ct) {
        return await _db.UnconfirmedUsers
            .Where(u =>  u.ExpiresAt <= utcNow)
            .ExecuteDeleteAsync(ct);
    }
}