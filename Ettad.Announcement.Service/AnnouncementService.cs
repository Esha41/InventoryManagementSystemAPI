using AutoMapper;
using Ettad.Application.Common.Interfaces;
using Ettad.Announcement.Service.Dtos;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Notification.Service;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using System.Text.Json;
using AnnouncementEntity = Ettad.Data.Entities.Announcement;
using Ettad.Data.Interfaces.Repositories;

namespace Ettad.Announcement.Service
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ICrossCuttingRepository<AnnouncementEntity> _announcementRepository;
        private readonly ICrossCuttingRepository<AnnouncementDismissal> _dismissalRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationHelperService _notificationHelper;
        private readonly IEffectiveRoleRepository _effectiveRoleService;

        public AnnouncementService(
            ICrossCuttingRepository<AnnouncementEntity> announcementRepository,
            ICrossCuttingRepository<AnnouncementDismissal> dismissalRepository,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper,
            ICurrentUserService currentUserService,
            INotificationHelperService notificationHelper,
            IEffectiveRoleRepository effectiveRoleService)
        {
            _announcementRepository = announcementRepository;
            _dismissalRepository = dismissalRepository;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _notificationHelper = notificationHelper;
            _effectiveRoleService = effectiveRoleService;
        }

        public async Task<APIOperationResponse<List<AnnouncementDto>>> GetAllAnnouncementsAsync()
        {
            try
            {
                var announcements = await _announcementRepository.FindAsync(
                    a => !a.IsDeleted,
                    false
                );

                var dtos = announcements
                    .OrderByDescending(a => a.CreationDate)
                    .Select(a => MapToDto(a))
                    .ToList();

                return APIOperationResponse<List<AnnouncementDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<AnnouncementDto>>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AnnouncementDto>> GetAnnouncementByIdAsync(long id)
        {
            try
            {
                var announcement = await _announcementRepository.FindOneAsync(
                    a => a.Id == id && !a.IsDeleted
                );

                if (announcement == null)
                {
                    return APIOperationResponse<AnnouncementDto>.Fail(
                        ResponseType.NotFound,
                        "Announcement not found");
                }

                var dto = MapToDto(announcement);
                return APIOperationResponse<AnnouncementDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AnnouncementDto>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ActiveAnnouncementDto>>> GetActiveAnnouncementsForUserAsync()
        {
            try
            {
                var unauthorized = GetCurrentUserIdOrFail<List<ActiveAnnouncementDto>>(out var userId);
                if (unauthorized != null)
                    return unauthorized;

                var now = _dateTimeProvider.Now;
           
                var startOfToday = now.Date;
                var startOfTomorrow = now.Date.AddDays(1);

                // Get all active announcements
                var announcements = await _announcementRepository.FindAsync(
                    a => !a.IsDeleted &&
                         a.IsActive &&
                         a.StartDate < startOfTomorrow &&
                         (a.EndDate == null || a.EndDate >= startOfToday),
                    false
                );

                // Get user's dismissals
                var dismissals = await _dismissalRepository.FindAsync(
                    d => d.UserId == userId,
                    false
                );

                var dismissedIds = new HashSet<long>(dismissals.Select(d => d.AnnouncementId));

                // Effective role ID(s) for role-targeted announcements (active session)
                var userRoleIds = (await _effectiveRoleService.GetEffectiveRoleIdsAsync(userId)).ToList();
                var userRoleIdSet = new HashSet<string>(userRoleIds, StringComparer.OrdinalIgnoreCase);

                // Filter announcements
                var activeAnnouncements = announcements
                    .Where(a =>
                    {
                        // Skip if dismissed and dismissable
                        if (a.IsDismissable && dismissedIds.Contains(a.Id))
                            return false;

                        // Check role targeting: if target roles specified, user must have at least one
                        if (!string.IsNullOrWhiteSpace(a.TargetRoles))
                        {
                            var targetRoleIds = ParseTargetRoleIds(a.TargetRoles);

                            if (targetRoleIds == null || targetRoleIds.Count == 0)
                                return true; // no valid target roles, show to all

                            return targetRoleIds.Any(rid => userRoleIdSet.Contains(rid));
                        }

                        return true; // no target roles = show to all
                    })
                    .OrderByDescending(a => a.Priority)
                    .ThenByDescending(a => a.CreationDate)
                    .Select(a => new ActiveAnnouncementDto
                    {
                        Id = a.Id,
                        Message = a.Message,
                        Priority = a.Priority,
                        DeliveryType = a.DeliveryType,
                        IsDismissable = a.IsDismissable
                    })
                    .ToList();

                return APIOperationResponse<List<ActiveAnnouncementDto>>.Success(activeAnnouncements);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<ActiveAnnouncementDto>>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AnnouncementDto>> CreateAnnouncementAsync(CreateAnnouncementDto dto)
        {
            try
            {
                var unauthorized = GetCurrentUserIdOrFail<AnnouncementDto>(out var createdBy);
                if (unauthorized != null)
                    return unauthorized;

                // Validate
                if (string.IsNullOrWhiteSpace(dto.Message))
                {
                    return APIOperationResponse<AnnouncementDto>.Fail(
                        ResponseType.BadRequest,
                        "Message is required");
                }

                if (dto.Message.Length > 500)
                {
                    return APIOperationResponse<AnnouncementDto>.Fail(
                        ResponseType.BadRequest,
                        "Message cannot exceed 500 characters");
                }

                if (dto.EndDate.HasValue && dto.EndDate < dto.StartDate)
                {
                    return APIOperationResponse<AnnouncementDto>.Fail(
                        ResponseType.BadRequest,
                        "End date cannot be before start date");
                }

                var announcement = new AnnouncementEntity
                {
                    Message = dto.Message,
                    Priority = dto.Priority,
                    DeliveryType = dto.DeliveryType,
                    IsDismissable = dto.IsDismissable,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    TargetRoles = dto.TargetRoles != null && dto.TargetRoles.Any()
                        ? JsonSerializer.Serialize(dto.TargetRoles)
                        : null,
                    IsActive = dto.IsActive,
                    CreationDate = _dateTimeProvider.Now,
                    CreatedBy = createdBy
                };

                var created = await _announcementRepository.AddAsync(announcement);
                var resultDto = MapToDto(created);

                if (dto.IsActive && dto.DeliveryType.HasFlag(AnnouncementDeliveryType.Notification))
                {
                    await SendAnnouncementNotificationAsync(created, createdBy);
                }

                return APIOperationResponse<AnnouncementDto>.Success(
                    resultDto,
                    "Announcement created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AnnouncementDto>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AnnouncementDto>> UpdateAnnouncementAsync(
            long id,
            UpdateAnnouncementDto dto)
        {
            try
            {
                var unauthorized = GetCurrentUserIdOrFail<AnnouncementDto>(out var updatedBy);
                if (unauthorized != null)
                    return unauthorized;

                var announcement = await _announcementRepository.FindOneAsync(
                    a => a.Id == id && !a.IsDeleted
                );

                if (announcement == null)
                {
                    return APIOperationResponse<AnnouncementDto>.Fail(
                        ResponseType.NotFound,
                        "Announcement not found");
                }

                // Update fields if provided
                if (dto.Message != null)
                {
                    if (string.IsNullOrWhiteSpace(dto.Message))
                    {
                        return APIOperationResponse<AnnouncementDto>.Fail(
                            ResponseType.BadRequest,
                            "Message cannot be empty");
                    }

                    if (dto.Message.Length > 500)
                    {
                        return APIOperationResponse<AnnouncementDto>.Fail(
                            ResponseType.BadRequest,
                            "Message cannot exceed 500 characters");
                    }

                    announcement.Message = dto.Message;
                }

                if (dto.Priority.HasValue)
                    announcement.Priority = dto.Priority.Value;

                if (dto.DeliveryType.HasValue)
                    announcement.DeliveryType = dto.DeliveryType.Value;

                if (dto.IsDismissable.HasValue)
                    announcement.IsDismissable = dto.IsDismissable.Value;

                if (dto.StartDate.HasValue)
                    announcement.StartDate = dto.StartDate.Value;

                if (dto.EndDate != null) // Allow setting to null
                    announcement.EndDate = dto.EndDate;

                if (dto.TargetRoles != null)
                {
                    announcement.TargetRoles = dto.TargetRoles.Any()
                        ? JsonSerializer.Serialize(dto.TargetRoles)
                        : null;
                }

                if (dto.IsActive.HasValue)
                    announcement.IsActive = dto.IsActive.Value;

                // Validate dates
                if (announcement.EndDate.HasValue && announcement.EndDate < announcement.StartDate)
                {
                    return APIOperationResponse<AnnouncementDto>.Fail(
                        ResponseType.BadRequest,
                        "End date cannot be before start date");
                }

                announcement.ModificationDate = _dateTimeProvider.Now;
                announcement.ModifiedBy = updatedBy;

                var updated = await _announcementRepository.UpdateAsync(announcement);
                var resultDto = MapToDto(updated);

                return APIOperationResponse<AnnouncementDto>.Success(
                    resultDto,
                    "Announcement updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AnnouncementDto>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAnnouncementAsync(long id)
        {
            try
            {
                var unauthorized = GetCurrentUserIdOrFail<bool>(out var deletedBy);
                if (unauthorized != null)
                    return unauthorized;

                var announcement = await _announcementRepository.FindOneAsync(
                    a => a.Id == id && !a.IsDeleted
                );

                if (announcement == null)
                {
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.NotFound,
                        "Announcement not found");
                }

                announcement.IsDeleted = true;
                announcement.DeletionDate = _dateTimeProvider.Now;
                announcement.DeletedBy = deletedBy;

                await _announcementRepository.UpdateAsync(announcement);

                return APIOperationResponse<bool>.Success(
                    true,
                    "Announcement deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DismissAnnouncementAsync(long announcementId)
        {
            try
            {
                var unauthorized = GetCurrentUserIdOrFail<bool>(out var userId);
                if (unauthorized != null)
                    return unauthorized;

                // Check if announcement exists and is dismissable
                var announcement = await _announcementRepository.FindOneAsync(
                    a => a.Id == announcementId && !a.IsDeleted
                );

                if (announcement == null)
                {
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.NotFound,
                        "Announcement not found");
                }

                if (!announcement.IsDismissable)
                {
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.BadRequest,
                        "This announcement cannot be dismissed");
                }

                // Check if already dismissed
                var existing = await _dismissalRepository.FindOneAsync(
                    d => d.AnnouncementId == announcementId && d.UserId == userId
                );

                if (existing != null)
                {
                    return APIOperationResponse<bool>.Success(
                        true,
                        "Announcement already dismissed");
                }

                // Create dismissal
                var dismissal = new AnnouncementDismissal
                {
                    AnnouncementId = announcementId,
                    UserId = userId,
                    DismissedAt = _dateTimeProvider.Now
                };

                await _dismissalRepository.AddAsync(dismissal);

                return APIOperationResponse<bool>.Success(
                    true,
                    "Announcement dismissed successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }

        public async Task ClearDismissalsForUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var dismissals = await _dismissalRepository.FindAsync(d => d.UserId == userId, false);
            if (dismissals != null && dismissals.Any())
            {
                foreach (var d in dismissals)
                    await _dismissalRepository.DeleteAsync(d);
            }
        }

        private async Task SendAnnouncementNotificationAsync(AnnouncementEntity announcement, string senderId)
        {
            try
            {
                var roleIds = ParseTargetRoleIds(announcement.TargetRoles);
                var hasTargetRoles = roleIds != null && roleIds.Count > 0;

                // When no target roles: broadcast to all users (like banner). When target roles: notify only those roles.
                await _notificationHelper.SendNotificationAsync(
                    title: "Announcement",
                    message: announcement.Message,
                    entityType: "Announcement",
                    entityId: announcement.Id,
                    userIds: null,
                    roleIds: hasTargetRoles ? roleIds : null,
                    senderId: senderId,
                    includeSuperAdmins: false,
                    includeAllUsers: !hasTargetRoles
                );
            }
            catch (Exception ex)
            {
                // Don't fail the announcement creation if notification dispatch fails
                System.Diagnostics.Debug.WriteLine($"Failed to send announcement notification: {ex.Message}");
            }
        }

        /// <summary>
        /// Parses TargetRoles JSON, handling both string and number elements (frontend may send numbers).
        /// Returns null if parsing fails or result is empty.
        /// </summary>
        private static List<string>? ParseTargetRoleIds(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.ValueKind != JsonValueKind.Array)
                    return null;

                var list = new List<string>();
                foreach (var el in root.EnumerateArray())
                {
                    var s = el.ValueKind switch
                    {
                        JsonValueKind.String => el.GetString(),
                        JsonValueKind.Number => el.TryGetInt64(out var n) ? n.ToString() : el.GetRawText(),
                        _ => null
                    };
                    if (!string.IsNullOrWhiteSpace(s))
                        list.Add(s.Trim());
                }
                return list.Count > 0 ? list : null;
            }
            catch
            {
                return null;
            }
        }

        private AnnouncementDto MapToDto(AnnouncementEntity entity)
        {
            return new AnnouncementDto
            {
                Id = entity.Id,
                Message = entity.Message,
                Priority = entity.Priority,
                DeliveryType = entity.DeliveryType,
                IsDismissable = entity.IsDismissable,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                TargetRoles = !string.IsNullOrWhiteSpace(entity.TargetRoles)
                    ? JsonSerializer.Deserialize<List<string>>(entity.TargetRoles)
                    : null,
                IsActive = entity.IsActive,
                CreationDate = entity.CreationDate,
                CreatedBy = entity.CreatedBy
            };
        }

        private APIOperationResponse<T>? GetCurrentUserIdOrFail<T>(out string userId)
        {
            userId = _currentUserService.UserId ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(userId))
                return null;

            return APIOperationResponse<T>.Fail(ResponseType.Unauthorized, "User not authenticated");
        }
    }
}
