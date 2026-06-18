using ElectronicJournal.API.Models;

namespace ElectronicJournal.API.Services;

public interface INotificationService
{
    Task<Notification> CreateAsync(int userId, string title, string message, NotificationType type);
    Task<IEnumerable<Notification>> GetForUserAsync(int userId);
    Task MarkAsReadAsync(int notificationId);
}
