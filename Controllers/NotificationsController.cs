using Microsoft.AspNetCore.Mvc;
using PatientApi.Services.Interfaces;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            return Ok(await _service.GetUserNotificationsAsync(userId));
        }

        [HttpPost("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _service.MarkAsReadAsync(notificationId);
            return NoContent();
        }
    }

}
