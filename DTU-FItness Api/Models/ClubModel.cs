using Microsoft.AspNetCore.SignalR;

public class ClubModel
{
    public Guid ClubID {get; set;}
    public string ClubName {get; set;}

    public ClubModel(string clubName)
    {
        ClubName = clubName ?? throw new ArgumentNullException(nameof(clubName), "ClubName is required.");
    }

    public string? Description {get; set;}

    public string? OwnerUserId {get; set;}
    
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}