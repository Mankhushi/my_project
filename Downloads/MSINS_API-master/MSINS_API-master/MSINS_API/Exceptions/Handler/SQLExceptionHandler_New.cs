using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MSINS_API.Exceptions.Handler
{
    internal sealed class SQLExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<SQLExceptionHandler> _logger;

        public SQLExceptionHandler(ILogger<SQLExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not SqlException sqlException)
            {
                return false;
            }

            var problemDetails = new ProblemDetails
            {
                Title = "Database Error",
                Detail = "A database error occurred while processing your request.",
                Status = httpContext.Response.StatusCode,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            // Add correlation ID
            if (httpContext.TraceIdentifier != null)
            {
                problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            }

            // Only include detailed error info in development
            if (httpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false)
            {
                problemDetails.Extensions["devErrorMessage"] = exception.Message;
                problemDetails.Extensions["sqlErrorNumber"] = sqlException.Number;
                problemDetails.Extensions["sqlState"] = sqlException.State;
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            _logger.LogError(sqlException,
                "SQL error occurred: {Title}. Error Number: {ErrorNumber}. State: {State}. Type: {Type}. Path: {Path}. TraceId: {TraceId}",
                problemDetails.Title,
                sqlException.Number,
                sqlException.State,
                sqlException.GetType().Name,
                httpContext.Request.Path,
                httpContext.TraceIdentifier);

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}