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
    var userNotifications = new List<UserNotification>();

    foreach (var member in clubMembers)
    {
        var notification = new Notification
        {
            EventID = clubEvent.EventID,
            Message = $"New event:  {clubEvent.Title}"
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();  // Save each notification to generate an ID

        var userNotification = new UserNotification
        {
            NotificationID = notification.NotificationID,
            IdentityUserID = member.MemberId,
            IsRead = false
        };

        userNotifications.Add(userNotification);
    }

    _context.UserNotifications.AddRange(userNotifications);
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
            Description = un.Notification.Event.Description,
            ClubName = un.Notification.Event.Club.ClubName,
        })
        .ToListAsync();

    return notifications;
}


}