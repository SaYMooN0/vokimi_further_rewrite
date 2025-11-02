using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using SharedKernel.auth;

namespace CoreVokiCreationService.Application.app_users.queries;

public sealed record GetCurrentUserQuery() : IQuery<AppUser>;

internal sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, AppUser>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserContext _userContext;

    public GetCurrentUserQueryHandler(IAppUsersRepository appUsersRepository, IUserContext userContext) {
        _appUsersRepository = appUsersRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<AppUser>> Handle(GetCurrentUserQuery query, CancellationToken ct) {
        return (await _appUsersRepository.GetByIdAsNoTracking(_userContext.AuthenticatedUserId, ct))!;
    }
}