using DtuFitnessApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DtuFitnessApi.Services;

public class ExerciseService
{
    private readonly ApplicationDbContext _context;

    public ExerciseService(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ExerciseLog> RegisterTrainingAsync(ExerciseLogDto logDto)
{
    // Find UserID from UserName
    var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Username == logDto.UserName);
    if (user == null)
        throw new ArgumentException("User not found.");

    // Find ExerciseID from ExerciseName
    var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Name == logDto.ExerciseName);
    if (exercise == null)
        throw new ArgumentException("Exercise not found.");

    var newLog = new ExerciseLog
    {
        UserID = user.IdentityUserID,
        ExerciseID = exercise.ExerciseID,
        ExerciseDate = logDto.ExerciseDate
    };

    _context.ExerciseLogs.Add(newLog);
    await _context.SaveChangesAsync();

    // Handle metrics
    foreach (var metricDto in logDto.Metrics)
    {
        var metric = await _context.Metrics.FirstOrDefaultAsync(m => m.Name == metricDto.Name);
        if (metric == null)
        {
            // Optionally create the metric if it doesn't exist
            metric = new Metric { Name = metricDto.Name };
            _context.Metrics.Add(metric);
            await _context.SaveChangesAsync();
        }

        var exerciseMetric = new ExerciseMetric
        {
            ExerciseLogID = newLog.LogID,
            MetricID = metric.MetricID,
            Value = metricDto.Value
        };

        _context.ExerciseMetrics.Add(exerciseMetric);
    }

    await _context.SaveChangesAsync();

    return newLog;
}


public async Task<ExerciseModel> CreateExerciseAsync(ExerciseCreateDto exerciseDto)
{
    if (string.IsNullOrWhiteSpace(exerciseDto.Name))
        throw new ArgumentException("Exercise name cannot be empty.");

    // Check if the exercise already exists
    var existingExercise = await _context.Exercises
                                         .FirstOrDefaultAsync(e => e.Name == exerciseDto.Name);
    if (existingExercise != null)
        throw new InvalidOperationException("An exercise with this name already exists.");

    var newExercise = new ExerciseModel { Name = exerciseDto.Name };
    _context.Exercises.Add(newExercise);
    await _context.SaveChangesAsync();

    return newExercise;
}




}
