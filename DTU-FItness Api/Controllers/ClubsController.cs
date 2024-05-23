using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DtuFitnessApi.Services;
using DtuFitnessApi.Models;
using DTU_FItness_Api.Models.ClubModels;




[ApiController]
[Route("api/[controller]")]
public class ClubsController : ControllerBase
{
    private readonly ClubService _clubService;
    private readonly NotificationService _notificationService;

    public ClubsController(ClubService clubService, NotificationService notificationService)
    {
        _clubService = clubService ?? throw new ArgumentNullException(nameof(clubService));
        _notificationService = notificationService;
    }

[HttpPost("create")]
[Authorize(Policy = "RequireAdminRole")]
public async Task<IActionResult> CreateClub([FromBody] ClubModel club)
{
    if (club == null) 
    {
        return BadRequest("Club cannot be null.");
    }

    try
    {
        // The service layer handles converting OwnerUsername to OwnerUserId
        var newClub = await _clubService.CreateClubAsync(club);
        // Return the created club. Depending on your implementation,
        // this could include the resolved OwnerUserId or just reflect the input model.
        return Ok(newClub);
    }
    catch (ArgumentException ex)
    {
        // This captures cases like missing username or club name, or if the user is not found.
        return BadRequest(ex.Message);
    }
    catch (Exception)
{
    // General error handling for unexpected exceptions.
    return StatusCode(500, "An error occurred while creating the club. Please try again later.");
}
}


[HttpPost("RegisterMember")]
[Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> RegisterMember([FromBody] RegisterMemberDto registerMemberDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (Success, Message) = await _clubService.RegisterMemberAsync(registerMemberDto.Username, registerMemberDto.ClubName);

        if (!Success)
        {
            return BadRequest(Message);
        }

        return Ok(Message);
    }

    [HttpPost("CreateEvent")]
    public async Task<IActionResult> CreateEvent([FromBody] EventCreationDto eventDto)
    {
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    try
    {
        // Pass the DTO directly to the service method
        var createdEvent = await _clubService.CreateEventAsync(eventDto);

        return CreatedAtAction(nameof(CreateEvent), new { id = createdEvent.EventID }, createdEvent);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}

    [HttpDelete("DeleteClub/{clubName}")]
    public async Task<IActionResult> DeleteClub(string clubName)
    {
    if (string.IsNullOrEmpty(clubName))
    {
        return BadRequest("Club name is required.");
    }

    bool deleted = await _clubService.DeleteClubByNameAsync(clubName);
    if (!deleted)
    {
        return NotFound("Club not found.");
    }

    return Ok("Club deleted successfully.");
    }


    [HttpPut("UpdateClubDescription/{clubName}")]
public async Task<IActionResult> UpdateClubDescription(string clubName, [FromBody] ClubDescriptionUpdateDto updateDto)
{
    if (string.IsNullOrWhiteSpace(updateDto.Description))
    {
        return BadRequest("The description cannot be empty.");
    }

    bool updated = await _clubService.UpdateClubDescriptionAsync(clubName, updateDto.Description);
    if (!updated)
    {
        return NotFound($"No club found with the name {clubName}.");
    }

    return Ok("Club description updated successfully.");
}

[HttpPut("ChangeClubOwner/{clubName}")]
public async Task<IActionResult> ChangeClubOwner(string clubName, [FromBody] ClubOwnerUpdateDto updateDto)
{
    if (string.IsNullOrEmpty(updateDto.NewOwnerUsername))
    {
        return BadRequest("New owner username must be provided.");
    }

    bool updated = await _clubService.UpdateClubOwnerByUsernameAsync(clubName, updateDto.NewOwnerUsername);
    if (!updated)
    {
        return NotFound($"Club named {clubName} not found or new owner username '{updateDto.NewOwnerUsername}' is invalid.");
    }

    return Ok("Club owner updated successfully.");
}

    [HttpGet("RetrivAllClubs")]
    public async Task<ActionResult<List<ClubDto>>> GetAllClubs()
    {
        var clubs = await _clubService.GetAllClubsAsync();
        return Ok(clubs);
    }

    [HttpGet("{clubId}/members")]
    public async Task<ActionResult<List<ClubMemberDto>>> GetClubMembers(string clubId)
    {
        var members = await _clubService.GetAllClubMembersAsync(clubId);
        if (members == null || !members.Any())
        {
            return NotFound("No members found for this club.");
        }
        return Ok(members);
    }
}
