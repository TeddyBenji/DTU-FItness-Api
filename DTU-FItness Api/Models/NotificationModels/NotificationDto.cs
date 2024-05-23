public class NotificationDto
{
    public int NotificationID { get; set; }
    public string Message { get; set; }
    public DateTime EventDate { get; set; }
    public string Description { get; set; }  // Add this line to include the event description
}

