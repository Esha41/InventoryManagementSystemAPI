using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities.Workflows;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Workflow.Service.DTO;
using Ettad.Workflow.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ettad.Workflow.Service.Imeplemention
{
    /// <summary>
    /// Service for managing workflow step notifiers
    /// </summary>
    public class WorkflowStepNotifierService : IWorkflowStepNotifierService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<WorkflowStepNotifierService> _logger;

        public WorkflowStepNotifierService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            ILogger<WorkflowStepNotifierService> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<List<WorkflowStepNotifierDto>>> GetNotifiersByStepIdAsync(int workflowStepId)
        {
            try
            {
                // Verify workflow step exists
                var workflowStep = await _context.WorkflowSteps
                    .FirstOrDefaultAsync(x => x.Id == workflowStepId);

                if (workflowStep == null)
                {
                    return APIOperationResponse<List<WorkflowStepNotifierDto>>.NotFound(
                        $"Workflow step with ID {workflowStepId} not found");
                }

                var notifiers = await _context.WorkflowStepNotifiers
                    .Where(x => x.WorkflowStepId == workflowStepId)
                    .Include(x => x.User)
                    .Include(x => x.Role)
                    .Select(x => new WorkflowStepNotifierDto
                    {
                        Id = x.Id,
                        WorkflowStepId = x.WorkflowStepId,
                        UserId = x.UserId,
                        RoleId = x.RoleId,
                        UserName = x.User != null ? x.User.UserName : null,
                        UserFullNameEn = x.User != null ? x.User.FullNameEN : null,
                        UserFullNameAr = x.User != null ? x.User.FullNameAR : null,
                        RoleName = x.Role != null ? x.Role.Name : null,
                        RoleNameAr = x.Role != null ? x.Role.NameAr : null
                    })
                    .ToListAsync();

                _logger.LogInformation(
                    "Retrieved {Count} notifiers for workflow step {WorkflowStepId}",
                    notifiers.Count,
                    workflowStepId);

                return APIOperationResponse<List<WorkflowStepNotifierDto>>.Success(notifiers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error retrieving notifiers for workflow step {WorkflowStepId}",
                    workflowStepId);
                return APIOperationResponse<List<WorkflowStepNotifierDto>>.BadRequest(
                    $"Error retrieving notifiers: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateStepNotifiersAsync(UpdateWorkflowStepNotifiersDto dto)
        {
            try
            {
                // Validate workflow step exists
                var workflowStep = await _context.WorkflowSteps
                    .FirstOrDefaultAsync(x => x.Id == dto.WorkflowStepId);

                if (workflowStep == null)
                {
                    return APIOperationResponse<bool>.NotFound(
                        $"Workflow step with ID {dto.WorkflowStepId} not found");
                }

                // Validate that UserIds and RoleIds are not both null
                if ((dto.UserIds == null || !dto.UserIds.Any()) &&
                    (dto.RoleIds == null || !dto.RoleIds.Any()))
                {
                    return APIOperationResponse<bool>.BadRequest(
                        "At least one UserId or RoleId must be provided");
                }

                // Validate users exist
                if (dto.UserIds != null && dto.UserIds.Any())
                {
                    var invalidUserIds = dto.UserIds
                        .Where(userId => !_context.Users.Any(u => u.Id == userId))
                        .ToList();

                    if (invalidUserIds.Any())
                    {
                        return APIOperationResponse<bool>.BadRequest(
                            $"Invalid user IDs: {string.Join(", ", invalidUserIds)}");
                    }
                }

                // Validate roles exist
                if (dto.RoleIds != null && dto.RoleIds.Any())
                {
                    var invalidRoleIds = dto.RoleIds
                        .Where(roleId => !_context.Roles.Any(r => r.Id == roleId))
                        .ToList();

                    if (invalidRoleIds.Any())
                    {
                        return APIOperationResponse<bool>.BadRequest(
                            $"Invalid role IDs: {string.Join(", ", invalidRoleIds)}");
                    }
                }

                // Remove existing notifiers for this step
                var existingNotifiers = await _context.WorkflowStepNotifiers
                    .Where(x => x.WorkflowStepId == dto.WorkflowStepId)
                    .ToListAsync();

                _context.WorkflowStepNotifiers.RemoveRange(existingNotifiers);

                // Add new notifiers
                var newNotifiers = new List<WorkflowStepNotifier>();
                var currentUser = _currentUserService.UserName ?? "SYSTEM";
                var now = DateTime.UtcNow;

                if (dto.UserIds != null && dto.UserIds.Any())
                {
                    foreach (var userId in dto.UserIds.Distinct())
                    {
                        newNotifiers.Add(new WorkflowStepNotifier
                        {
                            WorkflowStepId = dto.WorkflowStepId,
                            UserId = userId,
                            RoleId = null,
                            CreatedBy = currentUser,
                            CreationDate = now
                        });
                    }
                }

                if (dto.RoleIds != null && dto.RoleIds.Any())
                {
                    foreach (var roleId in dto.RoleIds.Distinct())
                    {
                        newNotifiers.Add(new WorkflowStepNotifier
                        {
                            WorkflowStepId = dto.WorkflowStepId,
                            UserId = null,
                            RoleId = roleId,
                            CreatedBy = currentUser,
                            CreationDate = now
                        });
                    }
                }

                await _context.WorkflowStepNotifiers.AddRangeAsync(newNotifiers);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Updated notifiers for workflow step {WorkflowStepId}. Added {UserCount} users and {RoleCount} roles. User: {User}",
                    dto.WorkflowStepId,
                    dto.UserIds?.Count ?? 0,
                    dto.RoleIds?.Count ?? 0,
                    currentUser);

                return APIOperationResponse<bool>.Success(true, "Workflow step notifiers updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating notifiers for workflow step {WorkflowStepId}",
                    dto.WorkflowStepId);
                return APIOperationResponse<bool>.BadRequest(
                    $"Error updating notifiers: {ex.Message}");
            }
        }

        public async Task<bool> AddNotifierAsync(CreateWorkflowStepNotifierDto dto)
        {
            try
            {
                // Validate that at least one list is provided and not empty
                if ((dto.UserIds == null || !dto.UserIds.Any()) &&
                    (dto.RoleIds == null || !dto.RoleIds.Any()))
                {
                    _logger.LogWarning(
                        "AddNotifierAsync called with no UserIds or RoleIds for workflow step {WorkflowStepId}",
                        dto.WorkflowStepId);
                    return false;
                }

                // Validate workflow step exists
                var workflowStep = await _context.WorkflowSteps
                    .FirstOrDefaultAsync(x => x.Id == dto.WorkflowStepId);

                if (workflowStep == null)
                {
                    _logger.LogWarning(
                        "Workflow step {WorkflowStepId} not found when adding notifiers",
                        dto.WorkflowStepId);
                    return false;
                }

                var currentUser = _currentUserService.UserName ?? "SYSTEM";
                var now = DateTime.UtcNow;
                var newNotifiers = new List<WorkflowStepNotifier>();

                // Process UserIds
                if (dto.UserIds != null && dto.UserIds.Any())
                {
                    // Validate all users exist
                    var invalidUserIds = dto.UserIds
                        .Where(userId => !_context.Users.Any(u => u.Id == userId))
                        .ToList();

                    if (invalidUserIds.Any())
                    {
                        _logger.LogWarning(
                            "Invalid user IDs provided when adding notifiers to workflow step {WorkflowStepId}: {InvalidUserIds}",
                            dto.WorkflowStepId,
                            string.Join(", ", invalidUserIds));
                        return false;
                    }

                    // Get existing notifiers for this step to check duplicates
                    var existingUserNotifiers = await _context.WorkflowStepNotifiers
                        .Where(x => x.WorkflowStepId == dto.WorkflowStepId && x.UserId != null)
                        .Select(x => x.UserId!)
                        .ToListAsync();

                    // Add new user notifiers (skip duplicates)
                    foreach (var userId in dto.UserIds.Distinct())
                    {
                        if (!existingUserNotifiers.Contains(userId))
                        {
                            newNotifiers.Add(new WorkflowStepNotifier
                            {
                                WorkflowStepId = dto.WorkflowStepId,
                                UserId = userId,
                                RoleId = null,
                                CreatedBy = currentUser,
                                CreationDate = now
                            });
                        }
                    }
                }

                // Process RoleIds
                if (dto.RoleIds != null && dto.RoleIds.Any())
                {
                    // Validate all roles exist
                    var invalidRoleIds = dto.RoleIds
                        .Where(roleId => !_context.Roles.Any(r => r.Id == roleId))
                        .ToList();

                    if (invalidRoleIds.Any())
                    {
                        _logger.LogWarning(
                            "Invalid role IDs provided when adding notifiers to workflow step {WorkflowStepId}: {InvalidRoleIds}",
                            dto.WorkflowStepId,
                            string.Join(", ", invalidRoleIds));
                        return false;
                    }

                    // Get existing notifiers for this step to check duplicates
                    var existingRoleNotifiers = await _context.WorkflowStepNotifiers
                        .Where(x => x.WorkflowStepId == dto.WorkflowStepId && x.RoleId != null)
                        .Select(x => x.RoleId!)
                        .ToListAsync();

                    // Add new role notifiers (skip duplicates)
                    foreach (var roleId in dto.RoleIds.Distinct())
                    {
                        if (!existingRoleNotifiers.Contains(roleId))
                        {
                            newNotifiers.Add(new WorkflowStepNotifier
                            {
                                WorkflowStepId = dto.WorkflowStepId,
                                UserId = null,
                                RoleId = roleId,
                                CreatedBy = currentUser,
                                CreationDate = now
                            });
                        }
                    }
                }

                // If no new notifiers to add (all were duplicates), return true
                if (!newNotifiers.Any())
                {
                    _logger.LogInformation(
                        "No new notifiers to add for workflow step {WorkflowStepId} (all were duplicates)",
                        dto.WorkflowStepId);
                    return true;
                }

                // Add all new notifiers
                await _context.WorkflowStepNotifiers.AddRangeAsync(newNotifiers);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Added {Count} notifiers to workflow step {WorkflowStepId}. Users: {UserCount}, Roles: {RoleCount}. User: {User}",
                    newNotifiers.Count,
                    dto.WorkflowStepId,
                    newNotifiers.Count(n => n.UserId != null),
                    newNotifiers.Count(n => n.RoleId != null),
                    currentUser);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error adding notifiers to workflow step {WorkflowStepId}",
                    dto.WorkflowStepId);
                return false;
            }
        }

        public async Task<APIOperationResponse<bool>> RemoveNotifierAsync(int notifierId)
        {
            try
            {
                var notifier = await _context.WorkflowStepNotifiers
                    .FirstOrDefaultAsync(x => x.Id == notifierId);

                if (notifier == null)
                {
                    return APIOperationResponse<bool>.NotFound(
                        $"Notifier with ID {notifierId} not found");
                }

                _context.WorkflowStepNotifiers.Remove(notifier);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Removed notifier {NotifierId} from workflow step {WorkflowStepId}. User: {User}",
                    notifierId,
                    notifier.WorkflowStepId,
                    _currentUserService.UserName);

                return APIOperationResponse<bool>.Success(true, "Notifier removed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error removing notifier {NotifierId}",
                    notifierId);
                return APIOperationResponse<bool>.BadRequest(
                    $"Error removing notifier: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<(List<string> UserIds, List<string> RoleIds)>> GetNotifierIdsByStepIdAsync(int workflowStepId)
        {
            try
            {
                var notifiers = await _context.WorkflowStepNotifiers
                    .Where(x => x.WorkflowStepId == workflowStepId)
                    .ToListAsync();

                var userIds = notifiers
                    .Where(x => x.UserId != null)
                    .Select(x => x.UserId!)
                    .Distinct()
                    .ToList();

                var roleIds = notifiers
                    .Where(x => x.RoleId != null)
                    .Select(x => x.RoleId!)
                    .Distinct()
                    .ToList();

                return APIOperationResponse<(List<string> UserIds, List<string> RoleIds)>.Success(
                    (userIds, roleIds));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error retrieving notifier IDs for workflow step {WorkflowStepId}",
                    workflowStepId);
                return APIOperationResponse<(List<string> UserIds, List<string> RoleIds)>.BadRequest(
                    $"Error retrieving notifier IDs: {ex.Message}");
            }
        }
    }
}

