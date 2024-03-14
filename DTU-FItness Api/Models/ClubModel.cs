using System;
using System.ComponentModel.DataAnnotations.Schema;

public class ClubModel
{
    public Guid ClubID { get; set; }
    public string ClubName { get; set; }
    public string? Description { get; set; }
    
    
    [NotMapped]
    public string OwnerUsername { get; set; }
   
    public Guid? OwnerUserId { get; set; } 
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}
