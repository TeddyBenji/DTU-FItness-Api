
namespace  DtuFitnessApi.Models
{
    


public class UserNotification
    {
        public int UserNotificationID { get; set; }
        public int NotificationID { get; set; }
        public string IdentityUserID { get; set; }
        public bool IsRead { get; set; }

        // Navigation properties
        public virtual Notification Notification { get; set; }
        public virtual UserProfile User { get; set; }
    }
}