using System;
using System.ComponentModel.DataAnnotations.Schema;
using DtuFitnessApi.Models;
using System.ComponentModel.DataAnnotations;


public class ClubModel
{
    [Key]
    [StringLength(36)]
    public string? ClubID { get; set; }

    [Required]
    [MaxLength(255)]
    public string ClubName { get; set; }

    public string? Description { get; set; }
    
    [NotMapped]
    public string OwnerUsername { get; set; }

    public string? OwnerUserId { get; set; }

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    // Initialize the collection to prevent null reference issues
    public virtual ICollection<ClubMember> ClubMembers { get; set; } = new HashSet<ClubMember>();
}



