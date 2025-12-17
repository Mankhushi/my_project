using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace MSINS_API.Exceptions.Handler
{
    public class SmtpExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<SmtpExceptionHandler> _logger;

        public SmtpExceptionHandler(ILogger<SmtpExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not SmtpException smtpException)
            {
                return false;
            }

            var problemDetails = new ProblemDetails
            {
                Title = "Email Service Error",
                Detail = "Unable to process email request at this time.",
                Status = StatusCodes.Status503ServiceUnavailable,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.4"
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
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
                problemDetails.Extensions["source"] = exception.Source;
                problemDetails.Extensions["smtpStatusCode"] = smtpException.StatusCode.ToString();
            }

            _logger.LogError(smtpException,
                "SMTP error occurred: {Title}. Status: {Status}. Type: {Type}. Path: {Path}. TraceId: {TraceId}",
                problemDetails.Title,
                smtpException.StatusCode,
                smtpException.GetType().Name,
                httpContext.Request.Path,
                httpContext.TraceIdentifier);

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}