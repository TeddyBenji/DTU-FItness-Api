using Microsoft.AspNetCore.Mvc;



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
public async Task<IActionResult> CreateClub([FromBody] ClubModel club)
{
    if (club == null) return BadRequest("Club cannot be null.");

    try
    {
        var newClub = await _clubService.CreateClubAsync(club);
        // Just return a successful response with the created club data
        return Ok(newClub); // You can also use StatusCode(201) if you want to specify that a resource was created without providing a location header.
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "An error occurred while creating the club. Please try again later.");
    }
}

}
