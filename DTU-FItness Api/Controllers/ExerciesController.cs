using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DtuFitnessApi.Models;
using DtuFitnessApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

    [HttpPost("CreateExercise")]
public async Task<IActionResult> CreateExercise([FromBody] ExerciseCreateDto exerciseDto)
{
    try
    {
        if (string.IsNullOrEmpty(exerciseDto.Name))
            return BadRequest("Exercise name is required.");

        var createdExercise = await _exerciseService.CreateExerciseAsync(exerciseDto);
        return CreatedAtAction(nameof(CreateExercise), new { id = createdExercise.ExerciseID }, createdExercise);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(ex.Message); // Handle specific known errors gracefully
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}"); // General error handling
    }
}

[HttpPost("CreateMetric")]
public async Task<IActionResult> CreateMetric([FromBody] MetricCreateDto metricDto)
{
    try
    {
        if (string.IsNullOrEmpty(metricDto.Name))
            return BadRequest("Metric name is required.");

        var createdMetric = await _exerciseService.CreateMetricAsync(metricDto);
        return CreatedAtAction(nameof(CreateMetric), new { id = createdMetric.MetricID }, createdMetric);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(ex.Message); // Handle specific known errors gracefully
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}"); // General error handling
    }
}

[HttpGet("GetWorkoutLogs")]
public async Task<IActionResult> GetWorkoutLogs()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userId))
    {
        return Unauthorized("User ID not found in token.");
    }

    var logs = await _exerciseService.GetExerciseLogsByUserIdAsync(userId);
    if (logs == null || logs.Count == 0)
    {
        return NotFound("No workout logs found for the user.");
    }

    return Ok(logs);
}




}

