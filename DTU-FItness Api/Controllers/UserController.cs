using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using DtuFitnessApi.Services;
using System.Security.Claims;

namespace DtuFitnessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure this controller requires authentication
    public class UserController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public UserController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Example method to fetch unread notifications for the authenticated user
        [HttpGet("unread-notifications")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            // Extract the user ID from the JWT Token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Adjust this based on how your JWT tokens are structured

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var notifications = await _notificationService.GetUnreadNotificationsForUser(userId);
            return Ok(notifications);
        }

        // Additional user-related methods can be added here...
    }
}
