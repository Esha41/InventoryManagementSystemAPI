using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Notification.Service.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using NotificationEntity = Ettad.Data.Entities.Notification;


namespace Ettad.Notification.Service
{
    public class NotificationService : INotificationService
    {
        private readonly ICrossCuttingRepository<NotificationEntity> _notificationRepository;
        private readonly ICrossCuttingRepository<NotificationReceiver> _receiverRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateNotificationDto> _validator;
        private readonly ICurrentUserService _currentUserService;

        public NotificationService(
            ICrossCuttingRepository<NotificationEntity> notificationRepository,
            ICrossCuttingRepository<NotificationReceiver> receiverRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            IValidator<CreateNotificationDto> validator,
            ICurrentUserService currentUserService)
        {
            _notificationRepository = notificationRepository;
            _receiverRepository = receiverRepository;
            _userManager = userManager;
            _roleManager = roleManager;
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
                var notification = new NotificationEntity
                {
                    Title = dto.Title,
                    Message = dto.Message,
                    EntityType = dto.EntityType,
                    EntityId = dto.EntityId,
                    CreationDate = DateTime.UtcNow,
                    CreatedBy = _currentUserService.UserId
                };

                // Get all user IDs to notify
                var userIdsToNotify = new HashSet<string>();

                // Add individual user IDs
                if (dto.UserIds != null && dto.UserIds.Any())
                {
                    foreach (var userId in dto.UserIds)
                    {
                        userIdsToNotify.Add(userId);
                    }
                }

                // Add users from roles
                if (dto.RoleIds != null && dto.RoleIds.Any())
                {
                    foreach (var roleId in dto.RoleIds)
                    {
                        var role = await _roleManager.FindByIdAsync(roleId);
                        if (role != null && !string.IsNullOrEmpty(role.Name))
                        {
                            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                            foreach (var user in usersInRole)
                            {
                                userIdsToNotify.Add(user.Id);
                            }
                        }
                    }
                }

                if (!userIdsToNotify.Any())
                {
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "At least one user or role must be specified");
                }

                // Save notification first
                var createdNotification = await _notificationRepository.AddAsync(notification);

                // Create receiver records
                var receivers = new List<NotificationReceiver>();
                foreach (var userId in userIdsToNotify)
                {
                    var receiver = new NotificationReceiver
                    {
                        NotificationId = createdNotification.Id,
                        UserId = userId,
                        RoleId = null,
                        IsRead = false
                    };
                    // Validate receiver: either UserId or RoleId must be set, but not both
                    ValidateReceiver(receiver);
                    receivers.Add(receiver);
                }

                // Also create role-based receiver records for tracking
                if (dto.RoleIds != null && dto.RoleIds.Any())
                {
                    foreach (var roleId in dto.RoleIds)
                    {
                        var roleReceiver = new NotificationReceiver
                        {
                            NotificationId = createdNotification.Id,
                            UserId = null,
                            RoleId = roleId,
                            IsRead = false
                        };
                        // Validate receiver: either UserId or RoleId must be set, but not both
                        ValidateReceiver(roleReceiver);
                        receivers.Add(roleReceiver);
                    }
                }

                // Save all receivers
                foreach (var receiver in receivers)
                {
                    await _receiverRepository.AddAsync(receiver);
                }

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
                    nameof(NotificationEntity.Receivers)
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
                receiver.ReadAt = DateTime.UtcNow;

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

                var now = DateTime.UtcNow;
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

        /// <summary>
        /// Validates that a NotificationReceiver has exactly one of UserId or RoleId set (not both, not neither).
        /// This replaces the database check constraint for better maintainability.
        /// </summary>
        private void ValidateReceiver(NotificationReceiver receiver)
        {
            var hasUserId = !string.IsNullOrEmpty(receiver.UserId);
            var hasRoleId = !string.IsNullOrEmpty(receiver.RoleId);

            if (!hasUserId && !hasRoleId)
            {
                throw new InvalidOperationException("NotificationReceiver must have either UserId or RoleId set, but both are null.");
            }

            if (hasUserId && hasRoleId)
            {
                throw new InvalidOperationException("NotificationReceiver cannot have both UserId and RoleId set. Only one should be provided.");
            }
        }
    }
}

