using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using DtuFitnessApi.Services;
using System.Security.Claims;

namespace DtuFitnessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly NotificationService _notificationService;
        private readonly UserService _userService;

        public UserController(NotificationService notificationService, UserService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
        }

        
        [HttpGet("unread-notifications")]
        public async Task<IActionResult> GetUnreadNotifications()
        {

            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var notifications = await _notificationService.GetUnreadNotificationsForUser(userId);
            return Ok(notifications);
        }

        [HttpPut("UpdateBio")]
        public async Task<IActionResult> UpdateBio([FromBody] BioUpdateDTO bioUpdate)
        {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
        return Unauthorized("User ID not found in token.");
        }

        if (bioUpdate == null || string.IsNullOrEmpty(bioUpdate.Bio))
        {
        return BadRequest("Bio content is required.");
        }

        var result = await _userService.UpdateBio(userId, bioUpdate.Bio);
        if (result)
        {
        return Ok("Bio updated successfully.");
        }
        else
        {
        return NotFound("User not found.");
        }
        }

    [HttpGet("GetBio")]
    public async Task<IActionResult> GetBio()
    {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId))
    {
        return Unauthorized("User ID not found in token.");
    }

    var bio = await _userService.GetBio(userId);
    if (bio == null)
    {
        return NotFound("User not found or no bio available.");
    }

    return Ok(bio);
    }


    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
    var users = await _userService.GetAllUsersAsync();
    if (users == null || !users.Any())
    {
        return NotFound("No users found.");
    }

    return Ok(users);
    }


   [HttpGet("GetUser/{username}")]
public async Task<IActionResult> GetUserByUsername(string username)
{
    if (string.IsNullOrEmpty(username))
    {
        return BadRequest("Username is required.");
    }

    var user = await _userService.GetUserByUsernameAsync(username);
    if (user == null)
    {
        return NotFound("User not found.");
    }

    return Ok(user);
}
 




        
    }
}
