public class EventCreationDto
{
    public string ClubName { get; set; } // Changed from ClubID to ClubName
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
}