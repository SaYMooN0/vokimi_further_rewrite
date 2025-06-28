using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedKernel.exceptions;

namespace ApiShared.middlewares;

internal class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger
    ) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        }
        catch (InvalidConstructorArgumentException ex) {
            await CustomResults.ErrorResponse(ex.Err).ExecuteAsync(context);
            return;
        }
        catch (UnexpectedBehaviourException ex) {
            var e = new Err(message: ex.Message, code: ex.ErrCode, ex.Details);
            await CustomResults.ErrorResponse(e).ExecuteAsync(context);
            return;
        }
        catch (Exception ex) {
            Err e = new(message: "Server error occurred. Please try again later");
            await CustomResults.ErrorResponse(e).ExecuteAsync(context);
            return;
        }
    }
}