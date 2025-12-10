using AuthService.Domain.unconfirmed_user_aggregate;

namespace AuthService.Application.common.repositories;

public interface IUnconfirmedUsersRepository
{
    Task<UnconfirmedUser?> GetByEmail(Email email, CancellationToken ct);
    Task Add(UnconfirmedUser unconfirmedUser, CancellationToken ct);
    Task Update(UnconfirmedUser unconfirmedUser, CancellationToken ct);
    Task Delete(UnconfirmedUser unconfirmedUser, CancellationToken ct);

    Task<UnconfirmedUser?> GetById(UnconfirmedUserId unconfirmedUserId, CancellationToken ct);
    Task<int> DeleteAllExpiredUsers(DateTime utcNow, CancellationToken ct);

}