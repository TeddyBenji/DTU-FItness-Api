using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DtuFitnessApi.Models;

public class ExerciseMetric
{
    [Key]
    public int ExerciseMetricID { get; set; } 

    [Required]
    public int ExerciseLogID { get; set; }

    [Required]
    public int MetricID { get; set; }

    [Required]
    public decimal Value { get; set; }

    [ForeignKey("ExerciseLogID")]
    public virtual ExerciseLog ExerciseLog { get; set; }

    [ForeignKey("MetricID")]
    public virtual Metric Metric { get; set; }
}