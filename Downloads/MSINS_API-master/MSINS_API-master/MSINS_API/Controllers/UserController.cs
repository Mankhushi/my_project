using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MSINS_API.Controllers
{
    [Route("msins/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Example endpoints for testing authorization policies

        [HttpGet("admin-only")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult AdminOnly()
        {
            return Ok("Access granted for admin users.");
        }

        [HttpGet("guest-only")]
        [Authorize(Policy = "GuestPolicy")]
        public IActionResult GuestOnly()
        {
            return Ok("Access granted for guest users.");
        }

        [HttpGet("admin-or-guest")]
        [Authorize(Policy = "AdminOrGuestPolicy")]
        public IActionResult AdminOrGuest()
        {
            return Ok("Access granted for admin or guest users.");
        }

        [HttpGet("all-roles")]
        [Authorize(Policy = "AllPolicy")]
        public IActionResult AllRoles()
        {
            return Ok("Access granted for all authorized users.");
        }
    }
}