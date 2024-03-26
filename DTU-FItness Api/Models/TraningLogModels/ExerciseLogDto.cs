public class ExerciseLogDto
{
    public string UserName { get; set; }
    public string ExerciseName { get; set; }
    public DateTime ExerciseDate { get; set; }
    public List<ExerciseMetricDto> Metrics { get; set; }
}
