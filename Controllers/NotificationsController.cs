using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientApi.Extensions;
using PatientApi.Services.Interfaces;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Patient,Doctor,Nurse,Admin")]
        public async Task<IActionResult> GetMyNotifications()
        {
            if (!User.TryGetUserId(out var userId)) return Unauthorized();
            return Ok(await _service.GetUserNotificationsAsync(userId));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            if (!User.TryGetUserId(out var currentUserId)) return Unauthorized();
            if (!User.IsInRole("Admin") && userId != currentUserId) return Forbid();
            return Ok(await _service.GetUserNotificationsAsync(userId));
        }

        [HttpPost("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            if (!User.TryGetUserId(out var userId)) return Unauthorized();
            var ok = await _service.MarkAsReadAsync(notificationId, userId);
            return ok ? NoContent() : NotFound();
        }
    }

}
