using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Common.Security;
using Ettad.Notification.Service;
using Ettad.Notification.Service.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Notification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ApiControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ICurrentUserService _currentUserService;

        public NotificationController(
            INotificationService notificationService,
            ICurrentUserService currentUserService)
        {
            _notificationService = notificationService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.NotificationsPage.View")]
        public async Task<IActionResult> GetUserNotifications([FromQuery] bool? isRead = null)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return ProcessResponse(APIOperationResponse<List<NotificationDto>>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, 
                    "User not authenticated"));
            }

            var result = await _notificationService.GetUserNotificationsAsync(userId, isRead);
            return ProcessResponse(result);
        }

        [HttpGet("unread-count")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.NotificationsPage.View")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return ProcessResponse(APIOperationResponse<int>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, 
                    "User not authenticated"));
            }

            var result = await _notificationService.GetUnreadCountAsync(userId);
            return ProcessResponse(result);
        }

        [HttpPatch("{id}/read")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.NotificationsPage.Edit")]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, 
                    "User not authenticated"));
            }

            var result = await _notificationService.MarkAsReadAsync(id, userId);
            return ProcessResponse(result);
        }

        [HttpPatch("read-all")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.NotificationsPage.Edit")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.Unauthorized, 
                    "User not authenticated"));
            }

            var result = await _notificationService.MarkAllAsReadAsync(userId);
            return ProcessResponse(result);
        }
    }
}

