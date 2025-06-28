

namespace SharedKernel.auth;

public interface IUserContext
{
    public const string TokenCookieKey= "_token";
    public const string UserIdContextKey= "appUserId";
    AppUserId AuthenticatedUserId { get; }
    ErrOr<AppUserId> UserIdFromToken();
}
