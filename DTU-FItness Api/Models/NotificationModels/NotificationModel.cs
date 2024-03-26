
namespace DtuFitnessApi.Models
{
public class Notification
    {
        public int NotificationID { get; set; }
        public int EventID { get; set; }
        public string Message { get; set; }

        // Navigation property back to the event
        public virtual Event Event { get; set; }

        // Collection for UserNotifications, if you're tracking read status per user
        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}