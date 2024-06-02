using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DtuFitnessApi.Models;
public class ExerciseLog
{
    [Key]
    public int LogID { get; set; }

    [Required]
    [StringLength(255)]
    public string UserID { get; set; }

    [Required]
    public int ExerciseID { get; set; }

    [Required]
    public DateTime ExerciseDate { get; set; } = DateTime.UtcNow;

    [ForeignKey("UserID")]
    public virtual UserProfile UserProfile { get; set; }

    [ForeignKey("ExerciseID")]
    public virtual ExerciseModel ExerciseModel { get; set; }

    public virtual ICollection<ExerciseMetric> ExerciseMetrics { get; set; } = new HashSet<ExerciseMetric>();
}
