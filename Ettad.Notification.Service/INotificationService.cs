using Ettad.Notification.Service.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Notification.Service
{
    public interface INotificationService
    {
        Task<APIOperationResponse<long>> CreateNotificationAsync(CreateNotificationDto dto);
        Task<APIOperationResponse<List<NotificationDto>>> GetUserNotificationsAsync(string userId, bool? isRead = null);
        Task<APIOperationResponse<bool>> MarkAsReadAsync(long notificationId, string userId);
        Task<APIOperationResponse<bool>> MarkAllAsReadAsync(string userId);
        Task<APIOperationResponse<int>> GetUnreadCountAsync(string userId);
    }
}

