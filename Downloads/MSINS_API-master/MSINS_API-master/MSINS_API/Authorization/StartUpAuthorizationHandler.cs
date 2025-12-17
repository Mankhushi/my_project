using Microsoft.AspNetCore.Authorization;

namespace MSINS_API.Authorization
{
    public class StartUpAuthorizationHandler : AuthorizationHandler<StartUpAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StartUpAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StartUpAuthorizationRequirement requirement)
        {
            var user = context.User;
            var roleClaim = user.FindFirst("Role")?.Value;
            var startupIdClaim = user.FindFirst("candidate_userId")?.Value;

            if (roleClaim == "Admin")
            {
                context.Succeed(requirement); // Admins have full access
                return Task.CompletedTask;
            }

            if (roleClaim == "StartUp" && !string.IsNullOrEmpty(startupIdClaim))
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var requestedStartupId = httpContext?.Request.RouteValues["startupId"]?.ToString();

                if (!string.IsNullOrEmpty(requestedStartupId) && requestedStartupId == startupIdClaim)
                {
                    context.Succeed(requirement); // Allow StartUp to access its own data
                    return Task.CompletedTask;
                }
            }

            context.Fail(); // Deny access if conditions aren't met
            return Task.CompletedTask;
        }
    }
}
