using DtuFitnessApi.Models;
using System.ComponentModel.DataAnnotations;


public class UserProfile
{
    
    [Key]
    public int ProfileID { get; set; }

    
    [StringLength(255)]
    public string IdentityUserID { get; set; }

    [Required]
    [MaxLength(255)]
    public string Username { get; set; }

    public string Email { get; set; }

    public string? Bio  { get; set; }

    public virtual ICollection<ClubMember> ClubMembers { get; set; }

    public virtual ICollection<ExerciseLog> ExerciseLogs { get; set; } = new HashSet<ExerciseLog>();

    public ICollection<UserNotification> UserNotifications { get; set; }

}

