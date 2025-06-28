using Microsoft.AspNetCore.Http;
using SharedKernel.auth;

namespace ApiShared.endpoints_filters;

internal class AuthenticationRequiredEndpointFilter : IEndpointFilter
{
    private readonly IUserContext _userContext;

    public AuthenticationRequiredEndpointFilter(IUserContext userContext) {
        _userContext = userContext;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        ErrOr<AppUserId> userIdOrErr = _userContext.UserIdFromToken();

        if (userIdOrErr.IsErr(out var err)) {
            return CustomResults.ErrorResponse(
                ErrFactory.AuthRequired("Access denied. Authentication required")
            );
        }


        return await next(context);
    }
}