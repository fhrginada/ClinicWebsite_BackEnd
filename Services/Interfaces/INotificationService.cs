public interface INotificationService
{
    Task SendAsync(int userId, string title, string message);
    Task<List<NotificationResponse>> GetUserNotificationsAsync(int userId);
    Task MarkAsReadAsync(int notificationId);
}
