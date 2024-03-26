using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DtuFitnessApi.Models;
using DtuFitnessApi.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController : ControllerBase
{
    private readonly ExerciseService _exerciseService;

    public ExerciseController(ExerciseService exerciseService)
    {
        _exerciseService = exerciseService ?? throw new ArgumentNullException(nameof(exerciseService));
    }

    [HttpPost("RegisterTraining")]
    [Authorize]
    public async Task<IActionResult> RegisterTraining([FromBody] ExerciseLogDto logDto) // Change to ExerciseLogDto
    {
        if (logDto == null) // Adjusted to logDto
        {
            return BadRequest("Log information cannot be null.");
        }

        try
        {
            // Now passing the correct DTO to the service method
            var registeredLog = await _exerciseService.RegisterTrainingAsync(logDto); // Adjusted to logDto
            return Ok(registeredLog);
        }
        catch (ArgumentException ex)
        {
            // Handle validation failures
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // Handle other unexpected exceptions
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}

