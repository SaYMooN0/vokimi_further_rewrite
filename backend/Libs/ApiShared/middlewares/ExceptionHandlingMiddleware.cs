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

    private static readonly Err ServerError = new("Server error occurred. Please try again later");

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        }
        catch (InvalidConstructorArgumentException ex) {
            _logger.LogCritical(ex.Err.ToString());
            await CustomResults.ErrorResponse(new Err("Server error. Please try again later")).ExecuteAsync(context);
            return;
        }
        catch (UnexpectedBehaviourException ex) {
            var e = new Err(message: ex.Message, code: ex.ErrCode, ex.Details);
            _logger.LogCritical(e.ToString());
            await CustomResults.ErrorResponse(ServerError).ExecuteAsync(context);
            return;
        }
        catch (Exception ex) {
            _logger.LogCritical(ex.ToString());

            await CustomResults.ErrorResponse(ServerError).ExecuteAsync(context);
            return;
        }
    }
}