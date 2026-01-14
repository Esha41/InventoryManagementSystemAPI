using AutoMapper;
using FluentValidation;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Notification.Service.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using NotificationEntity = Ettad.Data.Entities.Notification;


namespace Ettad.Notification.Service
{
    public class NotificationService : INotificationService
    {
        private readonly ICrossCuttingRepository<NotificationEntity> _notificationRepository;
        private readonly ICrossCuttingRepository<NotificationReceiver> _receiverRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateNotificationDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public NotificationService(
            ICrossCuttingRepository<NotificationEntity> notificationRepository,
            ICrossCuttingRepository<NotificationReceiver> receiverRepository,
            IUserService userService,
            IMapper mapper,
            IValidator<CreateNotificationDto> validator,
            ICurrentUserService currentUserService)
        {
            _notificationRepository = notificationRepository;
            _receiverRepository = receiverRepository;
            _userService = userService;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
        }

        public async Task<APIOperationResponse<long>> CreateNotificationAsync(CreateNotificationDto dto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Create notification entity
                // CreatedBy: Audit trail - who/what created this record (could be null for system jobs)
                // SenderId: Business field - who the notification appears to be from (null = system notification)
                var notification = new NotificationEntity
                {
                    Title = dto.Title,
                    Message = dto.Message,
                    EntityType = dto.EntityType,
                    EntityId = dto.EntityId,
                    SenderId = dto.SenderId, // Don't default - let caller decide (null = system notification)
                    CreationDate = DateTime.Now,
                    CreatedBy = _currentUserService.UserId // Audit: who created the record (null for system jobs)
                };

                var recipientsResult = await ResolveRecipientsAsync(dto);
                if (!recipientsResult.Succeeded)
                {
                    return APIOperationResponse<long>.Fail(
                        ResolveResponseType(recipientsResult.StatusCode),
                        recipientsResult.Message ?? "Unable to resolve notification recipients.");
                }

                var userIdsToNotify = recipientsResult.Data ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                if (!userIdsToNotify.Any())
                {
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "At least one user or role must be specified");
                }

                notification.Receivers = userIdsToNotify
                    .Select(userId => new NotificationReceiver
                    {
                        UserId = userId,
                        IsRead = false
                    })
                    .ToList();

                // Save notification with receivers in a single transaction
                var createdNotification = await _notificationRepository.AddAsync(notification);

                return APIOperationResponse<long>.Success(createdNotification.Id, "Notification created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<NotificationDto>>> GetUserNotificationsAsync(string userId, bool? isRead = null)
        {
            try
            {
                // Get all notifications where user is a receiver
                var receivers = await _receiverRepository.FindAsync(
                    r => r.UserId == userId,
                    false
                );

                if (isRead.HasValue)
                {
                    receivers = receivers.Where(r => r.IsRead == isRead.Value).ToList();
                }

                var notificationIds = receivers.Select(r => r.NotificationId).Distinct().ToList();

                if (!notificationIds.Any())
                {
                    return APIOperationResponse<List<NotificationDto>>.Success(new List<NotificationDto>());
                }

                // Get notifications
                var notifications = await _notificationRepository.FindAsync(
                    n => notificationIds.Contains(n.Id) && !n.IsDeleted,
                    false,
                    nameof(NotificationEntity.Receivers),
                    nameof(NotificationEntity.Sender)
                );

                // Map to DTOs with user-specific read status
                var notificationDtos = new List<NotificationDto>();
                foreach (var notification in notifications.OrderByDescending(n => n.CreationDate))
                {
                    var userReceiver = receivers.FirstOrDefault(r => r.NotificationId == notification.Id && r.UserId == userId);
                    var dto = _mapper.Map<NotificationDto>(notification);
                    dto.IsRead = userReceiver?.IsRead ?? false;
                    dto.ReadAt = userReceiver?.ReadAt;
                    notificationDtos.Add(dto);
                }

                return APIOperationResponse<List<NotificationDto>>.Success(notificationDtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<NotificationDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> MarkAsReadAsync(long notificationId, string userId)
        {
            try
            {
                var receiver = await _receiverRepository.FindOneAsync(
                    r => r.NotificationId == notificationId && r.UserId == userId
                );

                if (receiver == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Notification receiver not found");
                }

                receiver.IsRead = true;
                receiver.ReadAt = DateTime.Now;

                await _receiverRepository.UpdateAsync(receiver);

                return APIOperationResponse<bool>.Success(true, "Notification marked as read");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> MarkAllAsReadAsync(string userId)
        {
            try
            {
                var receivers = await _receiverRepository.FindAsync(
                    r => r.UserId == userId && !r.IsRead,
                    false
                );

                if (!receivers.Any())
                {
                    return APIOperationResponse<bool>.Success(true, "No unread notifications");
                }

                var now = DateTime.Now;
                foreach (var receiver in receivers)
                {
                    receiver.IsRead = true;
                    receiver.ReadAt = now;
                    await _receiverRepository.UpdateAsync(receiver);
                }

                return APIOperationResponse<bool>.Success(true, "All notifications marked as read");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<int>> GetUnreadCountAsync(string userId)
        {
            try
            {
                var receivers = await _receiverRepository.FindAsync(
                    r => r.UserId == userId && !r.IsRead,
                    false
                );

                var count = receivers.Count();
                return APIOperationResponse<int>.Success(count);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<int>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<APIOperationResponse<HashSet<string>>> ResolveRecipientsAsync(CreateNotificationDto dto)
        {
            var recipients = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (dto.UserIds != null)
            {
                foreach (var userId in dto.UserIds.Where(id => !string.IsNullOrWhiteSpace(id)))
                {
                    recipients.Add(userId);
                }
            }

            if (dto.RoleIds != null && dto.RoleIds.Any())
            {
                var usersByRole = await _userService.GetByRoleIdsAsync(dto.RoleIds);
                if (!usersByRole.Succeeded)
                {
                    return APIOperationResponse<HashSet<string>>.Fail(
                        ResolveResponseType(usersByRole.StatusCode),
                        usersByRole.Message ?? "Unable to fetch users for the provided roles.");
                }

                if (usersByRole.Data != null)
                {
                    foreach (var user in usersByRole.Data.Where(u => !string.IsNullOrWhiteSpace(u.Id)))
                    {
                        recipients.Add(user.Id);
                    }
                }
            }

            if (dto.IncludeSuperAdmins)
            {
                var superAdminsResult = await _userService.GetSuperAdminsAsync();
                if (!superAdminsResult.Succeeded)
                {
                    return APIOperationResponse<HashSet<string>>.Fail(
                        ResolveResponseType(superAdminsResult.StatusCode),
                        superAdminsResult.Message ?? "Unable to fetch super administrators.");
                }

                if (superAdminsResult.Data != null)
                {
                    foreach (var user in superAdminsResult.Data.Where(u => !string.IsNullOrWhiteSpace(u.Id)))
                    {
                        recipients.Add(user.Id);
                    }
                }
            }

            return APIOperationResponse<HashSet<string>>.Success(recipients);
        }

        private static ResponseType ResolveResponseType(int statusCode)
        {
            return Enum.IsDefined(typeof(ResponseType), statusCode)
                ? (ResponseType)statusCode
                : ResponseType.BadRequest;
        }

    }
}

