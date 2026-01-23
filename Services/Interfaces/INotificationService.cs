namespace PatientApi.Services.Interfaces
{
    using PatientApi.Models.ViewModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

public interface INotificationService
{
    Task SendAsync(int userId, string title, string message);
    Task<List<NotificationResponse>> GetUserNotificationsAsync(int userId);
    Task<bool> MarkAsReadAsync(int notificationId, int actingUserId);
}
}
