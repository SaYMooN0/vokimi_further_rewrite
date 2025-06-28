using ApiShared.endpoints_filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ApiShared.extensions;

public static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder WithGroupAuthenticationRequired(this RouteGroupBuilder builder) {
        return builder.AddEndpointFilter<AuthenticationRequiredEndpointFilter>();
    }
}
