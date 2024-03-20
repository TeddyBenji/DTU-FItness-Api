using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;




[ApiController]
[Route("api/[controller]")]
public class ClubsController : ControllerBase
{
    private readonly ClubService _clubService;

    public ClubsController(ClubService clubService)
    {
        _clubService = clubService ?? throw new ArgumentNullException(nameof(clubService));
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
    catch (Exception ex)
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



}
