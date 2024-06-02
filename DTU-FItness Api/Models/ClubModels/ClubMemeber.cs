
namespace DtuFitnessApi.Models
{
public class ClubMember
{
    public int ClubMemberId { get; set; } 
    public string ClubId { get; set; } 
    public string MemberId { get; set; } 
    
    public virtual ClubModel Club { get; set; }
    public virtual UserProfile UserProfile { get; set; }
}

}
