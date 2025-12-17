using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Exceptions.HandlerClass;

namespace MSINS_API.Exceptions.Handler
{
    internal sealed class ValidationExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ValidationExceptionHandler> _logger;

        public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not ValidationExceptions validationException)
            {
                return false;
            }

            var problemDetails = new ProblemDetails
            {
                Title = "Validation Error",
                Detail = validationException.Message,
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            // Add validation error details
            if (validationException is BaseException baseException && baseException.AdditionalData?.Count > 0)
            {
                foreach (var data in baseException.AdditionalData)
                {
                    problemDetails.Extensions[data.Key] = data.Value;
                }
            }

            // Add correlation ID
            if (httpContext.TraceIdentifier != null)
            {
                problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            }

            _logger.LogError(validationException,
                "Validation error occurred: {Title}. Type: {Type}. Path: {Path}. TraceId: {TraceId}",
                problemDetails.Title,
                validationException.GetType().Name,
                httpContext.Request.Path,
                httpContext.TraceIdentifier);

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}