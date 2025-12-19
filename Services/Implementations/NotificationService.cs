using PatientApi.Models.Entities;
using PatientApi.Models.ViewModels;
using PatientApi.Services.Interfaces;


namespace PatientApi.Services.Implementations
{
    
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;

        public NotificationService(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task SendAsync(int userId, string title, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message
            };

            await _repo.AddAsync(notification);
        }

        public async Task<List<NotificationResponse>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _repo.GetByUserIdAsync(userId);

            return notifications.Select(n => new NotificationResponse
            {
                Id = n.NotificationId,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            }).ToList();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            await _repo.MarkAsReadAsync(notificationId);
        }
    }

}
