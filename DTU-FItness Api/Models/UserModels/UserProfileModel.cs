using DtuFitnessApi.Models;
using System.ComponentModel.DataAnnotations;


public class UserProfile
{
    // Include the ProfileID which is the primary key with auto-increment
    [Key]
    public int ProfileID { get; set; }

    // Use string type for GUIDs stored as VARCHAR
    [StringLength(255)]
    public string IdentityUserID { get; set; }

    [Required]
    [MaxLength(255)]
    public string Username { get; set; }

    // Include other properties like Bio and FitnessGoals if needed

    public virtual ICollection<ClubMember> ClubMembers { get; set; }

    public virtual ICollection<ExerciseLog> ExerciseLogs { get; set; } = new HashSet<ExerciseLog>();
}

