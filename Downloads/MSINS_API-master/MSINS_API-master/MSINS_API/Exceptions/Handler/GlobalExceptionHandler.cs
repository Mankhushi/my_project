using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using MSINS_API.Exceptions.HandlerClass;

namespace MSINS_API.Exceptions.Handler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path,
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            if (exception is BaseException baseException)
            {
                problemDetails.Status = (int)baseException.StatusCode;
                problemDetails.Title = baseException.Message;
                problemDetails.Detail = baseException.ToString();

                if (baseException.AdditionalData?.Count > 0)
                {
                    foreach (var data in baseException.AdditionalData)
                    {
                        problemDetails.Extensions[data.Key] = data.Value;
                    }
                }
            }
            else
            {
                problemDetails.Title = "An unexpected error occurred";
                problemDetails.Detail = "Something went wrong. Please try again later.";

                // Only include actual error message in development
                if (httpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false)
                {
                    problemDetails.Extensions["devErrorMessage"] = exception.Message;
                    problemDetails.Extensions["stackTrace"] = exception.StackTrace;
                    problemDetails.Extensions["source"] = exception.Source;
                }
            }

            // Add correlation ID if available
            if (httpContext.TraceIdentifier != null)
            {
                problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            }

            _logger.LogError(exception, 
                "An error occurred: {Title}. Type: {Type}. Path: {Path}. TraceId: {TraceId}", 
                problemDetails.Title,
                exception.GetType().Name,
                httpContext.Request.Path,
                httpContext.TraceIdentifier);

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            
            return true;
        }
    }
}