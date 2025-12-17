using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Exceptions.HandlerClass;

namespace MSINS_API.Exceptions.Handler
{
    internal sealed class InternalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<InternalExceptionHandler> _logger;

        public InternalExceptionHandler(ILogger<InternalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not InternalException internalException)
            {
                return false;
            }

            var problemDetails = new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred. Please try again later.",
                Status = StatusCodes.Status500InternalServerError,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            // Add additional data if available
            if (internalException is BaseException baseException && baseException.AdditionalData?.Count > 0)
            {
                foreach (var data in baseException.AdditionalData)
                {
                    problemDetails.Extensions[data.Key] = data.Value;
                }
            }

            // Only include detailed error info in development
            if (httpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false)
            {
                problemDetails.Extensions["devErrorMessage"] = exception.Message;
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
                problemDetails.Extensions["source"] = exception.Source;
            }

            // Add correlation ID
            if (httpContext.TraceIdentifier != null)
            {
                problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            }

            _logger.LogCritical(internalException,
                "Internal server error: {Title}. Type: {Type}. Path: {Path}. TraceId: {TraceId}",
                problemDetails.Title,
                internalException.GetType().Name,
                httpContext.Request.Path,
                httpContext.TraceIdentifier);

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}