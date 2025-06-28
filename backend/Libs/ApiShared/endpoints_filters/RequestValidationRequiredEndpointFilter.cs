using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ApiShared.endpoints_filters;

internal class RequestValidationRequiredEndpointFilter<T> : IEndpointFilter
    where T : class, IRequestWithValidationNeeded
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) {
        var httpContext = context.HttpContext;

        //for not application/json type requests
        bool isContentJson =
            httpContext.Request.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) ??
            false;
        if (!isContentJson) {
            return CustomResults.ErrorResponse(ErrFactory.IncorrectFormat(
                "Invalid Content-Type. Expected application/json"
            ));
        }

        //for empty body requests
        if (httpContext.Request.ContentLength == 0) {
            return CustomResults.ErrorResponse(ErrFactory.NoValue.Common(
                "Request body cannot be empty when Content-Type is application/json"
            ));
        }

        T? request = null;

        try {
            request = await httpContext.Request.ReadFromJsonAsync<T>();
        }
        catch (JsonException exception) {
            return CustomResults.ErrorResponse(new Err("Unable to read from json"));
        }

        if (request is null || request is not T validatableRequest) {
            return CustomResults.ErrorResponse(ErrFactory.IncorrectFormat("Invalid request body format"));
        }

        ErrOrNothing validationResult = validatableRequest.Validate();
        if (validationResult.IsErr(out var err)) {
            return CustomResults.ErrorResponse(err);
        }

        httpContext.Items["validatedRequest"] = validatableRequest;

        return await next(context);
    }
}