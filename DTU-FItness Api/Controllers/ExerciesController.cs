using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DtuFitnessApi.Models;
using DtuFitnessApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DTU_FItness_Api.Models;

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
    public async Task<IActionResult> RegisterTraining([FromBody] ExerciseLogDto logDto) 
    {
        if (logDto == null) 
        {
            return BadRequest("Log information cannot be null.");
        }

        try
        {
           
            var registeredLog = await _exerciseService.RegisterTrainingAsync(logDto); 
            return Ok(registeredLog);
        }
        catch (ArgumentException ex)
        {
            
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            
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
        return BadRequest(ex.Message); 
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}"); 
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
        return BadRequest(ex.Message); 
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}"); 
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

    [HttpGet("Get/exercises")]
    public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAllExercises()
    {
        var exercises = await _exerciseService.GetAllExercisesAsync();
        return Ok(exercises);
    }

    [HttpGet("Get/Metric")]

    public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAllMetric()
    {
        var metric = await _exerciseService.GetAllMetricsAsync();
        return Ok(metric);
    }

}

