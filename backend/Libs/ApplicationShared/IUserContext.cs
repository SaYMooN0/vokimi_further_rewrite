using SharedKernel;
using SharedKernel.domain.ids;
using SharedKernel.errs;

namespace ApplicationShared;

public interface IUserContext
{
    public const string TokenCookieKey = "_token";
    public const string UserIdContextKey = "appUserId";
    AppUserId AuthenticatedUserId { get; }
    IAuthenticatedUserContext AuthenticatedUser { get; }
    ErrOr<AppUserId> UserIdFromToken();
}