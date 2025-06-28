using ApiShared.endpoints_filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ApiShared.extensions;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<T>(this RouteHandlerBuilder builder) where T
        : class, IRequestWithValidationNeeded
        => builder.AddEndpointFilter<RequestValidationRequiredEndpointFilter<T>>();

    public static RouteHandlerBuilder WithAuthenticationRequired(this RouteHandlerBuilder builder) =>
        builder.AddEndpointFilter<AuthenticationRequiredEndpointFilter>();
}