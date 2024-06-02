
namespace DtuFitnessApi.Models
{
public class Notification
    {
        public int NotificationID { get; set; }
        public int EventID { get; set; }
        public string Message { get; set; }
        public virtual Event Event { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}