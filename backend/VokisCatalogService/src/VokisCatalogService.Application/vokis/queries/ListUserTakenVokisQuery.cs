using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record ListUserTakenVokisQuery() :
    IQuery<UserTakenVokisList>,
    IWithAuthCheckStep;

internal sealed class ListUserTakenVokisQueryHandler : IQueryHandler<ListUserTakenVokisQuery, UserTakenVokisList>
{
    private readonly IUserContext _userContext;
    private readonly IAppUsersRepository _appUsersRepository;

    public ListUserTakenVokisQueryHandler(IUserContext userContext, IAppUsersRepository appUsersRepository) {
        _userContext = userContext;
        _appUsersRepository = appUsersRepository;
    }

    public async Task<ErrOr<UserTakenVokisList>> Handle(ListUserTakenVokisQuery query, CancellationToken ct) {
        AppUser? user =
            await _appUsersRepository.GetUserWithTakenVokisAsNoTracking(_userContext.AuthenticatedUserId, ct);
        if (user is null) {
            return ErrFactory.NotFound.User("Cannot find user account", $"User id: {_userContext.AuthenticatedUserId}");
        }

        return user.TakenVokis;
    }
}