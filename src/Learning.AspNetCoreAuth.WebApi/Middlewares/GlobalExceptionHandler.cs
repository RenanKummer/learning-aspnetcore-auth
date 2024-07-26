using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Learning.AspNetCoreAuth.WebApi.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception has been thrown");

        if (!httpContext.Response.HasStarted)
        {
            var responseBody = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = (int)responseBody.Status;
            await httpContext.Response.WriteAsJsonAsync(responseBody, cancellationToken: cancellationToken);
        }

        return true;
    }
}