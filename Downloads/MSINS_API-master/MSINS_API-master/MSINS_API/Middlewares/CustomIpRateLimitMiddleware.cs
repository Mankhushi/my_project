using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace MSINS_API.Middlewares
{
    public class CustomRateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly PartitionedRateLimiter<HttpContext> _rateLimiter;
        private static readonly ConcurrentDictionary<string, int> _requestCounts = new();


        public CustomRateLimitMiddleware(RequestDelegate next, PartitionedRateLimiter<HttpContext> rateLimiter)
        {
            _next = next;
            _rateLimiter = rateLimiter;
        }

        //public async Task InvokeAsync(HttpContext context)
        //{
        //    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        //    var endpoint = context.Request.Path.ToString().ToLower();
        //    var key = $"{ipAddress}:{endpoint}"; // Unique key per IP and endpoint

        //    // Increment request count
        //    _requestCounts.AddOrUpdate(key, 1, (_, count) => count + 1);

        //    var lease = await _rateLimiter.AcquireAsync(context, 1);

        //    if (!lease.IsAcquired)
        //    {
        //        context.Response.StatusCode = 429; // Too Many Requests
        //        context.Response.ContentType = "application/json";
        //        context.Response.Headers["Retry-After"] = "60"; // Retry after 60 seconds

        //        var responseMessage = new
        //        {
        //            message = "Custom message: Rate limit exceeded. Please try again later.",
        //            used_count = _requestCounts[key], // Current request count
        //            limit = 5 // Max allowed
        //        };

        //        await context.Response.WriteAsync(JsonSerializer.Serialize(responseMessage));
        //        return;
        //    }

        //    context.Response.Headers["X-RateLimit-Limit"] = "5"; // Allowed requests
        //    context.Response.Headers["X-RateLimit-Used"] = _requestCounts[key].ToString(); // Used requests

        //    await _next(context);

        //    // Reset count after 60 seconds (optional cleanup)
        //    _ = Task.Delay(TimeSpan.FromSeconds(60)).ContinueWith(_ => _requestCounts.TryRemove(key, out _));

        //}
    }

}
