using SharedKernel.auth;

namespace AuthService.Application.app_users.queries;

public sealed record PingUserAuthQuery() : IQuery<AppUserId>;

internal sealed class PingUserAuthQueryHandler : IQueryHandler<PingUserAuthQuery, AppUserId>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserContext _userContext;

    public PingUserAuthQueryHandler(IAppUsersRepository appUsersRepository, IUserContext userContext) {
        _appUsersRepository = appUsersRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<AppUserId>> Handle(PingUserAuthQuery query, CancellationToken ct) {
        var idFromToken = _userContext.UserIdFromToken();
        if (idFromToken.IsErr(out var err)) {
            return ErrFactory.AuthRequired("User is not authenticated", err.Message);
        }

        bool doesUserExist = await _appUsersRepository.AnyUserWithId(idFromToken.AsSuccess());
        if (!doesUserExist) {
            return ErrFactory.AuthRequired("Unexpected login state. Pleas log out and log in again");
        }

        return idFromToken.AsSuccess();
    }
}