using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DtuFitnessApi.Models;

public class Metric
{
    [Key]
    public int MetricID { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    public virtual ICollection<ExerciseMetric> ExerciseMetrics { get; set; } = new HashSet<ExerciseMetric>();
}
