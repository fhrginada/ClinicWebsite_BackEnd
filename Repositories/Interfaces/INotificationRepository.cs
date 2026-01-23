using PatientApi.Models.Entities;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task<Notification?> GetByIdAsync(int notificationId);
    Task<List<Notification>> GetByUserIdAsync(int userId);
    Task MarkAsReadAsync(int notificationId);
}
