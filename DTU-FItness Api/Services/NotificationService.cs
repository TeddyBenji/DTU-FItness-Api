using DtuFitnessApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class NotificationService
{
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateNotificationsForEvent(Event clubEvent)
    {
        var clubMembers = await _context.ClubMembers
                                         .Where(cm => cm.ClubId == clubEvent.ClubID)
                                         .ToListAsync();

        var notifications = new List<Notification>();

        foreach (var member in clubMembers)
        {
            var notification = new Notification
            {
                EventID = clubEvent.EventID,
                Message = $"New event: {clubEvent.Title} on {clubEvent.EventDate}"
            };

            notifications.Add(notification);

            // Optionally, directly create UserNotification entries here
        }

        _context.Notifications.AddRange(notifications);
        await _context.SaveChangesAsync();
    }

    public async Task<List<NotificationDto>> GetUnreadNotificationsForUser(string userId)
{
    var notifications = await _context.UserNotifications
        .Where(un => un.IdentityUserID == userId && !un.IsRead)
        .Select(un => new NotificationDto
        {
            NotificationID = un.Notification.NotificationID,
            Message = un.Notification.Message,
            EventDate = un.Notification.Event.EventDate,
            // Add other relevant fields from the Notification entity
        })
        .ToListAsync();

    return notifications;
}


}