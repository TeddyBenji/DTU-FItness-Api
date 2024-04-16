using System.ComponentModel.DataAnnotations;

public class MetricCreateDto
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
}
