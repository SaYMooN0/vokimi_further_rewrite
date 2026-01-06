using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ApiShared.extensions;

public static class EndpointsExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<T>(
        this RouteHandlerBuilder builder
    ) where T : class, IRequestWithValidationNeeded =>
        builder.AddEndpointFilter<RequestValidationRequiredEndpointFilter<T>>();
}