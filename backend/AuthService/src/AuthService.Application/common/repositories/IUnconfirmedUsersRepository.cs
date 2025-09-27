using AuthService.Domain.unconfirmed_user_aggregate;

namespace AuthService.Application.common.repositories;

public interface IUnconfirmedUsersRepository
{
    Task<UnconfirmedUser?> GetByEmail(Email email);
    Task Add(UnconfirmedUser unconfirmedUser);
    Task Update(UnconfirmedUser unconfirmedUser);
    Task Delete(UnconfirmedUser unconfirmedUser);

    Task<UnconfirmedUser?> GetById(UnconfirmedUserId unconfirmedUserId);
}