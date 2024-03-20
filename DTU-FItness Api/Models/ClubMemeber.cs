
namespace DtuFitnessApi.Models
{
public class ClubMember
{
    public int ClubMemberId { get; set; } // Primary key for the ClubMember entity
    public string ClubId { get; set; } // Foreign key referencing the Clubs table should be a string
    public string MemberId { get; set; } // Foreign key referencing the UserProfiles table should be a string
    
    // Navigation properties
    public virtual ClubModel Club { get; set; }
    public virtual UserProfile UserProfile { get; set; }
}

}
