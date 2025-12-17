using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Exceptions.HandlerClass;

namespace MSINS_API.Exceptions.Handler
{
    internal sealed class NotFoundExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<NotFoundExceptionHandler> _logger;

        public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not NotFoundException notFoundException)
            {
                return false;
            }

            var problemDetails = new ProblemDetails
            {
                Title = "Resource Not Found",
                Detail = notFoundException.Message,
                Status = StatusCodes.Status404NotFound,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            };

            // Add additional data if available
            if (notFoundException is BaseException baseException && baseException.AdditionalData?.Count > 0)
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

            _logger.LogWarning(notFoundException,
                "Resource not found: {Title}. Type: {Type}. Path: {Path}. TraceId: {TraceId}",
                problemDetails.Title,
                notFoundException.GetType().Name,
                httpContext.Request.Path,
                httpContext.TraceIdentifier);

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}