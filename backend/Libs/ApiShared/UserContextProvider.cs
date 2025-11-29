using ApplicationShared;
using InfrastructureShared.Auth;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace ApiShared;

internal class UserContextProvider : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenParser _tokenParser;

    public UserContextProvider(IHttpContextAccessor httpContextAccessor, ITokenParser tokenParser) {
        _httpContextAccessor = httpContextAccessor;
        _tokenParser = tokenParser;
    }

    public AppUserId AuthenticatedUserId =>
        TryGetUserIdFromContextItems(out var userId) ? userId : UserIdFromToken().AsSuccess();

    public ErrOr<AppUserId> UserIdFromToken() {
        if (!TryGetTokenFromCookie(out string? token) || string.IsNullOrEmpty(token)) {
            return ErrFactory.AuthRequired("User is not authenticated", "Log in to your account");
        }

        ErrOr<AppUserId> errOrId = _tokenParser.UserIdFromJwtToken(new JwtTokenString(token));
        if (errOrId.IsErr(out var err)) {
            return err;
        }

        AppUserId userId = errOrId.AsSuccess();
        if (_httpContextAccessor.HttpContext is not null) {
            _httpContextAccessor.HttpContext.Items[IUserContext.UserIdContextKey] = userId;
        }

        return userId;
    }

    private bool TryGetTokenFromCookie(out string? token) {
        if (_httpContextAccessor.HttpContext is not null) {
            return _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(IUserContext.TokenCookieKey, out token);
        }

        token = null;
        return false;
    }

    private bool TryGetUserIdFromContextItems(out AppUserId userId) {
        if (
            _httpContextAccessor.HttpContext is not null
            && _httpContextAccessor.HttpContext.Items.TryGetValue(IUserContext.UserIdContextKey, out var userIdObj)
            && userIdObj is AppUserId typedId
        ) {
            userId = typedId;
            return true;
        }

        userId = default!;
        return false;
    }
}