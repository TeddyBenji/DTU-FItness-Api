using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DtuFitnessApi.Models;
public class ExerciseModel
{
    [Key]
    public int ExerciseID { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    public virtual ICollection<ExerciseLog> ExerciseLogs { get; set; } = new HashSet<ExerciseLog>();
}
