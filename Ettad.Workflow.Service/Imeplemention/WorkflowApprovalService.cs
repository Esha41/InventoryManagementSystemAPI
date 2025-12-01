using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Notification.Service;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Workflow.Service.Interface;
using Ettad.Workflows.Service.DTO;
using Ettad.Workflows.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Imeplemention
{
    public class WorkflowApprovalService:IWorkflowApprovalService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly IWorkflowStepNotifierService _workflowStepNotifierService;
        private readonly ILogger<WorkflowApprovalService> _logger;

        public WorkflowApprovalService(
            ApplicationDbContext context, 
            ICurrentUserService currentUserService, 
            INotificationHelperService notificationHelperService,
            IWorkflowStepNotifierService workflowStepNotifierService,
            ILogger<WorkflowApprovalService> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _notificationHelperService = notificationHelperService;
            _workflowStepNotifierService = workflowStepNotifierService;
            _logger = logger;
        }
     
        public async Task<IEnumerable<WorkflowApprovalStepDto>> GetAllAsync()
        {
            return await _context.WorkflowApprovalSteps
                .Select(x => new WorkflowApprovalStepDto
                {
                    Id = x.Id,
                    WorkflowStepId = x.WorkflowStepId,
                    TargetRequestId = x.TargetRequestId,
                    RequestType = x.RequestType,
                    ApproverUserId = x.ApproverUserId,
                    IsDelegation = x.IsDelegation,
                    ApprovedDate = x.ApprovedDate,
                    Status = x.Status,
                    Comments = x.Comments,
                    IsCurrent = x.IsCurrent
                }).ToListAsync();
        }

        public async Task<WorkflowApprovalStepDto> GetByIdAsync(int id)
        {
            var entity = await _context.WorkflowApprovalSteps.FindAsync(id);
            if (entity == null) return null;

            return new WorkflowApprovalStepDto
            {
                Id = entity.Id,
                WorkflowStepId = entity.WorkflowStepId,
                TargetRequestId = entity.TargetRequestId,
                RequestType = entity.RequestType,
                ApproverUserId = entity.ApproverUserId,
                IsDelegation = entity.IsDelegation,
                ApprovedDate = entity.ApprovedDate,
                Status = entity.Status,
                Comments = entity.Comments,
                IsCurrent = entity.IsCurrent
            };
        }

        public async Task<WorkflowApprovalStepDto> CreateAsync(CreateWorkflowApprovalStepDto dto)
        {
            var entity = new WorkflowApprovalStep
            {
                WorkflowStepId = dto.WorkflowStepId,
                TargetRequestId = dto.TargetRequestId,
                RequestType = dto.RequestType,
                ApproverUserId = dto.ApproverUserId,
                IsDelegation = dto.IsDelegation,               
                Status = dto.Status,
                Comments = dto.Comments,
                IsCurrent = dto.IsCurrent,
                CreationDate = DateTime.Now,
                CreatedBy = dto.CreatedBy
            };

            _context.WorkflowApprovalSteps.Add(entity);
            await _context.SaveChangesAsync();

            return new WorkflowApprovalStepDto
            {
                Id = entity.Id,
                WorkflowStepId = entity.WorkflowStepId,
                TargetRequestId = entity.TargetRequestId,
                RequestType = entity.RequestType,
                ApproverUserId = entity.ApproverUserId,
                IsDelegation = entity.IsDelegation,
                ApprovedDate = entity.ApprovedDate,
                Status = entity.Status,
                Comments = entity.Comments,
                IsCurrent = entity.IsCurrent,
                CreatedBy = entity.CreatedBy
            };
        }

        public async Task<WorkflowApprovalStepDto> UpdateAsync(int id, UpdateWorkflowApprovalStepDto dto)
        {
            var entity = await _context.WorkflowApprovalSteps.FindAsync(id);
            if (entity == null) return null;

            entity.WorkflowStepId = dto.WorkflowStepId;
            entity.TargetRequestId = dto.TargetRequestId;
            entity.RequestType = dto.RequestType;
            entity.ApproverUserId = dto.ApproverUserId;
            entity.IsDelegation = dto.IsDelegation;
            entity.ApprovedDate = dto.ApprovedDate;
            entity.Status = dto.Status;
            entity.Comments = dto.Comments;
            entity.IsCurrent = dto.IsCurrent;
            entity.ModificationDate = DateTime.Now;
            entity.ModifiedBy = dto.ChangedBy;

            await _context.SaveChangesAsync();

            return new WorkflowApprovalStepDto
            {
                Id = entity.Id,
                WorkflowStepId = entity.WorkflowStepId,
                TargetRequestId = entity.TargetRequestId,
                RequestType = entity.RequestType,
                ApproverUserId = entity.ApproverUserId,
                IsDelegation = entity.IsDelegation,
                ApprovedDate = entity.ApprovedDate,
                Status = entity.Status,
                Comments = entity.Comments,
                IsCurrent = entity.IsCurrent,
                ChangedBy = entity.ModifiedBy,
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.WorkflowApprovalSteps.FindAsync(id);
            if (entity == null) return false;

            _context.WorkflowApprovalSteps.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WorkflowApprovalWithOrderDto>> GetOrdersWithApprovalStepsAsync()
        {
            var currentUserId = _currentUserService.UserId;

            // 1. Get all role IDs of current user
            var userRoleIds = await _context.Set<IdentityUserRole<string>>()
                .Where(ur => ur.UserId == currentUserId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            if (!userRoleIds.Any())
                return Enumerable.Empty<WorkflowApprovalWithOrderDto>();

            // 2. Query workflow approval steps joined with workflow steps and base requests
            var query = from ws in _context.WorkflowApprovalSteps
                        join br in _context.BaseRequests
                            on ws.TargetRequestId equals br.Id
                        join wfs in _context.WorkflowSteps
                            on ws.WorkflowStepId equals wfs.Id
                        where
                            // Step assigned directly to this user
                            ws.ApproverUserId == currentUserId
                            // OR user role matches main approver
                            || userRoleIds.Contains(wfs.ApplicationRoleId)
                            // OR user role matches higher approval
                            || (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId) && userRoleIds.Contains(wfs.HigherApprovalRoleId))

                        select new WorkflowApprovalWithOrderDto
                        {
                            // WorkflowStep properties
                            WorkflowId = wfs.WorkflowId,
                            StepOrder = wfs.StepOrder,
                            ApplicationRoleId = wfs.ApplicationRoleId,
                            ApplicationEntityId = wfs.ApplicationEntityId,
                            RequireHigherApproval = wfs.RequireHigherApproval,
                            HigherApprovalRoleId = wfs.HigherApprovalRoleId,
                            HigherApplicationEntityId = wfs.HigherApplicationEntityId,
                            ReserveQty = wfs.ReserveQty,

                            // WorkflowApprovalStep properties
                            WorkflowStepId = ws.WorkflowStepId,
                            TargetRequestId = ws.TargetRequestId,
                            RequestType = ws.RequestType,
                            ApprovedDate = ws.ApprovedDate,
                            Status = ws.Status,
                            Comments = ws.Comments,
                            IsCurrent = ws.IsCurrent,
                            ApproverUserId = ws.ApproverUserId,

                            // BaseRequest properties
                            BaseRequestId = br.Id,
                            RequestNo = br.RequestNo,
                            BaseRequestType = br.RequestType,
                            Reason = br.Reason,
                            Priority = br.Priority,
                            BaseRequestStatus = br.Status,
                            Notes = br.Notes,
                            DepartmentId = br.DepartmentId,
                            RequesterId = br.RequesterId,
                            RequestPurposeId = br.RequestPurposeId,
                            RequestDate =br.CreationDate,

                            WorkflowApprovalStepId = ws.Id
                        };

            return await query.ToListAsync();
        }

        public async Task<APIOperationResponse<bool>> ProcessActionAsync(ApproveRejectWorkflowApprovalDto model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get current step
                var currentStep = await GetCurrentApprovalStepByRequestIdAsync(model.BaseRequestID);
                if (currentStep == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "No current workflow step found for this request.");

                // Store original status before modification
                var oldStatus = currentStep.Status;

                // Set comments
                currentStep.Comments = model.Comments;

                // Call respective method
                switch (model.Action)
                {
                    case RequestStatus.Approved:
                        await ApproveStepAsync(currentStep, model);
                        break;

                    case RequestStatus.Rejected:
                        await RejectStepAsync(currentStep, model);
                        break;

                    //case RequestStatus.Returned:
                    //    await ReturnStepAsync(currentStep, model);
                    //    break;

                    default:
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Invalid workflow action.");
                }

                // Log action with original status
                await LogStepActionAsync(currentStep.Id, currentStep.WorkflowStepId, oldStatus, model.Action, model.Comments, _currentUserService.UserName);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // 🔔 Send notifications to step notifiers (after transaction commits successfully)
                await SendNotificationsToStepNotifiersAsync(currentStep.WorkflowStepId, model);

                return APIOperationResponse<bool>.Success(true);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<WorkflowApprovalStepDto> ApproveAsync(ApproveRejectWorkflowApprovalDto dto)
        {
            // Get the step ID before processing
            var currentStep = await GetCurrentApprovalStepByRequestIdAsync(dto.BaseRequestID);
            if (currentStep == null)
            {
                throw new KeyNotFoundException("No current workflow step found for this request.");
            }

            var stepId = currentStep.Id;

            var result = await ProcessActionAsync(dto);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Message ?? "Approval failed");
            }

            // Get the updated step after processing by ID
            var updatedStep = await _context.WorkflowApprovalSteps
                .Include(x => x.WorkflowStep)
                .FirstOrDefaultAsync(x => x.Id == stepId);

            if (updatedStep == null)
            {
                throw new KeyNotFoundException("Approval step not found after processing");
            }

            return MapToDto(updatedStep);
        }

        public async Task<WorkflowApprovalStepDto> RejectAsync(ApproveRejectWorkflowApprovalDto dto)
        {
            // Get the step ID before processing
            var currentStep = await GetCurrentApprovalStepByRequestIdAsync(dto.BaseRequestID);
            if (currentStep == null)
            {
                throw new KeyNotFoundException("No current workflow step found for this request.");
            }

            var stepId = currentStep.Id;

            var result = await ProcessActionAsync(dto);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Message ?? "Rejection failed");
            }

            // Get the updated step after processing by ID
            var updatedStep = await _context.WorkflowApprovalSteps
                .Include(x => x.WorkflowStep)
                .FirstOrDefaultAsync(x => x.Id == stepId);

            if (updatedStep == null)
            {
                throw new KeyNotFoundException("Approval step not found after processing");
            }

            return MapToDto(updatedStep);
        }

        public async Task<WorkflowApprovalStepDto> ApproveOrReject(ApproveRejectWorkflowApprovalDto dto)
        {
            // Validate action
            if (dto.Action != RequestStatus.Approved && dto.Action != RequestStatus.Rejected)
            {
                throw new InvalidOperationException($"Invalid action: {dto.Action}. Only 'Approved' or 'Rejected' actions are allowed.");
            }

            // Get the step ID before processing
            var currentStep = await GetCurrentApprovalStepByRequestIdAsync(dto.BaseRequestID);
            if (currentStep == null)
            {
                throw new KeyNotFoundException($"No current workflow step found for request ID {dto.BaseRequestID}. The request may have already been processed or does not exist.");
            }

            var stepId = currentStep.Id;
            var actionName = dto.Action == RequestStatus.Approved ? "approve" : "reject";

            try
            {
                var result = await ProcessActionAsync(dto);
                if (!result.Succeeded)
                {
                    var errorMessage = result.Message ?? $"Failed to {actionName} the workflow step.";
                    throw new InvalidOperationException(errorMessage);
                }

                // Get the updated step after processing by ID
                var updatedStep = await _context.WorkflowApprovalSteps
                    .Include(x => x.WorkflowStep)
                    .FirstOrDefaultAsync(x => x.Id == stepId);

                if (updatedStep == null)
                {
                    throw new KeyNotFoundException($"Workflow approval step with ID {stepId} was not found after processing. The step may have been removed.");
                }

                return MapToDto(updatedStep);
            }
            catch (Exception ex) when (!(ex is KeyNotFoundException || ex is InvalidOperationException))
            {
                // Wrap unexpected exceptions with context
                throw new InvalidOperationException($"An error occurred while attempting to {actionName} the workflow step: {ex.Message}", ex);
            }
        }

        private WorkflowApprovalStepDto MapToDto(WorkflowApprovalStep entity)
        {
            return new WorkflowApprovalStepDto
            {
                Id = entity.Id,
                WorkflowStepId = entity.WorkflowStepId,
                TargetRequestId = entity.TargetRequestId,
                RequestType = entity.RequestType,
                ApproverUserId = entity.ApproverUserId,
                IsDelegation = entity.IsDelegation,
                ApprovedDate = entity.ApprovedDate,
                Status = entity.Status,
                Comments = entity.Comments,
                IsCurrent = entity.IsCurrent,
                ChangedBy = entity.ModifiedBy,
            };
        }

        // Approve step
        private async Task ApproveStepAsync(WorkflowApprovalStep step, ApproveRejectWorkflowApprovalDto model)
        {
            // Prevent double approval/rejection
            if (step.Status == RequestStatus.Approved || step.Status == RequestStatus.Rejected)
            {
                throw new Exception("This step has already been processed.");
            }

            // Only current step can be processed
            if (!step.IsCurrent)
            {
                throw new Exception("This step is not active anymore.");
            }

            step.Status = RequestStatus.Approved;
            step.ApproverUserId = _currentUserService.UserId;
            step.ApprovedDate = DateTime.UtcNow;
            step.IsCurrent = false;
            step.ModifiedBy = _currentUserService.UserId;
            step.ModificationDate = DateTime.UtcNow;

            var baseRequest = await _context.BaseRequests
                .FirstOrDefaultAsync(x => x.Id == step.TargetRequestId);

            if (baseRequest == null)
                return;

            // ⭐ Higher approval only when SendToHigherApproval = true
            if (model.SendToHigherApproval == true)
            {
                bool created = await HandleHigherApprovalAsync(step, baseRequest);

                if (created)
                {
                    // STOP here — do NOT send requester notification
                    return;
                }
            }

            // Get approver info for email notifications
            var approver = await _context.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);
            var approverName = approver?.FullNameEN ?? approver?.FullNameAR ?? approver?.UserName ?? "Approver";
            var stepNumber = step.WorkflowStep?.StepOrder ?? 0;
            var requestDepartmentId = baseRequest.DepartmentId > 0 ? (long?)baseRequest.DepartmentId : null;

            // 📧 Send confirmation email to current approver (fire and forget - don't block)
            // Only send if department matches for restricted roles, otherwise send normally
            _ = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation(
                        "Checking if approval confirmation email should be sent to approver {ApproverId} for request {RequestId}, DepartmentId: {DeptId}",
                        _currentUserService.UserId, baseRequest.Id, requestDepartmentId);

                    bool shouldSendEmail = await ShouldSendEmailToUserAsync(_currentUserService.UserId, requestDepartmentId);
                    
                    _logger.LogInformation(
                        "ShouldSendEmailToUserAsync returned {ShouldSend} for approver {ApproverId}",
                        shouldSendEmail, _currentUserService.UserId);

                    if (shouldSendEmail)
                    {
                        _logger.LogInformation(
                            "Sending approval confirmation email to approver {ApproverId} for request {RequestId}",
                            _currentUserService.UserId, baseRequest.Id);

                        await _notificationHelperService.SendNotificationAndEmailAsync(
                            $"Approval Confirmation - Request {baseRequest.RequestNo}",
                            $"You have successfully approved {baseRequest.RequestType} request {baseRequest.RequestNo} at step {stepNumber}." +
                            (!string.IsNullOrWhiteSpace(model.Comments) ? $" Your comments: {model.Comments}" : ""),
                            "Request",
                            baseRequest.Id,
                            new List<string> { _currentUserService.UserId },
                            null,
                            _currentUserService.UserId
                        );

                        _logger.LogInformation(
                            "Approval confirmation email sent successfully to approver {ApproverId} for request {RequestId}",
                            _currentUserService.UserId, baseRequest.Id);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Skipping approval confirmation email to approver {ApproverId} - department mismatch for restricted role or other condition",
                            _currentUserService.UserId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, 
                        "Failed to send approval confirmation email to approver {ApproverId} for request {RequestId}. Error: {ErrorMessage}",
                        _currentUserService.UserId, baseRequest.Id, ex.Message);
                }
            });

            // ⭐ Continue normal workflow
            var workflowSteps = await _context.WorkflowSteps
                .Where(ws => ws.WorkflowId == step.WorkflowStep.WorkflowId)
                .OrderBy(ws => ws.StepOrder)
                .ToListAsync();

            var nextStep = workflowSteps.FirstOrDefault(ws => ws.StepOrder > step.WorkflowStep.StepOrder);

            if (nextStep != null)
            {
                var nextApproval = new WorkflowApprovalStep
                {
                    WorkflowStepId = nextStep.Id,
                    TargetRequestId = step.TargetRequestId,
                    RequestType = step.RequestType,
                    Status = RequestStatus.New,
                    IsCurrent = true,
                    CreatedBy = _currentUserService.UserId,
                    CreationDate = DateTime.UtcNow
                };

                _context.WorkflowApprovalSteps.Add(nextApproval);
                baseRequest.Status = RequestStatus.UnderProcess;

                // 📧 Send email to requester about step approval (fire and forget - don't block)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _notificationHelperService.SendNotificationAndEmailAsync(
                            $"Request {baseRequest.RequestNo} - Step {stepNumber} Approved",
                            $"Your {baseRequest.RequestType} request {baseRequest.RequestNo} has been approved at step {stepNumber} by {approverName}." +
                            (!string.IsNullOrWhiteSpace(model.Comments) ? $" Comments: {model.Comments}" : ""),
                            "Request",
                            baseRequest.Id,
                            new List<string> { baseRequest.CreatedBy },
                            null,
                            _currentUserService.UserId
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send approval notification email to requester for request {RequestId}", baseRequest.Id);
                    }
                });

                // Send notification to next step approvers
                // Filter by department for restricted roles, otherwise send to all users in role
                var nextRoles = new List<string> { nextStep.ApplicationRoleId };
                if (!string.IsNullOrEmpty(nextStep.HigherApprovalRoleId))
                    nextRoles.Add(nextStep.HigherApprovalRoleId);

                // Separate restricted and non-restricted roles
                var (restrictedRoleIds, nonRestrictedRoleIds) = await SeparateRestrictedRolesAsync(nextRoles);

                _logger.LogInformation(
                    "Next approver notification: Restricted roles: {RestrictedCount}, Non-restricted roles: {NonRestrictedCount}, Request DepartmentId: {RequestDeptId}",
                    restrictedRoleIds.Count, nonRestrictedRoleIds.Count, requestDepartmentId);

                // For restricted roles: get filtered user IDs by department
                var restrictedUserIds = new List<string>();
                if (restrictedRoleIds.Any() && requestDepartmentId.HasValue)
                {
                    restrictedUserIds = await GetFilteredApproverUserIdsAsync(restrictedRoleIds, requestDepartmentId);
                    _logger.LogInformation(
                        "Next approver notification: Found {Count} users in restricted roles after department filtering",
                        restrictedUserIds.Count);
                }
                else if (restrictedRoleIds.Any() && !requestDepartmentId.HasValue)
                {
                    _logger.LogWarning(
                        "Next approver notification: Request has no department ID, skipping restricted role users for request {RequestId}",
                        baseRequest.Id);
                }

                // Combine restricted user IDs and non-restricted role IDs
                if (restrictedUserIds.Any() || nonRestrictedRoleIds.Any())
                {
                    _logger.LogInformation(
                        "Next approver notification: Sending to {RestrictedUserCount} restricted users and {NonRestrictedRoleCount} non-restricted roles for request {RequestId}",
                        restrictedUserIds.Count, nonRestrictedRoleIds.Count, baseRequest.Id);

                    await _notificationHelperService.SendNotificationAndEmailAsync(
                        "New Approval Required",
                        $"A {baseRequest.RequestType} request {baseRequest.RequestNo} is pending your approval at step {nextStep.StepOrder}. Please review and take action.",
                        "Request",
                        baseRequest.Id,
                        restrictedUserIds.Any() ? restrictedUserIds : null,
                        nonRestrictedRoleIds.Any() ? nonRestrictedRoleIds : null,
                        _currentUserService.UserId
                    );
                }
                else
                {
                    _logger.LogWarning(
                        "Next approver notification: No next approvers found (after department filtering) for request {RequestId} at step {StepOrder}. Restricted users: {RestrictedCount}, Non-restricted roles: {NonRestrictedCount}",
                        baseRequest.Id, nextStep.StepOrder, restrictedUserIds.Count, nonRestrictedRoleIds.Count);
                }

                // 🔔 Also notify next step notifiers if configured
                await SendNotificationsToStepNotifiersOnWorkflowStartAsync(nextStep.Id, baseRequest.Id);
            }
            else
            {
                // ⭐ Final approval — notify requester
                baseRequest.Status = RequestStatus.Approved;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _notificationHelperService.SendNotificationAndEmailAsync(
                            $"Request {baseRequest.RequestNo} - Fully Approved",
                            $"Your {baseRequest.RequestType} request {baseRequest.RequestNo} has been fully approved by {approverName}." +
                            (!string.IsNullOrWhiteSpace(model.Comments) ? $" Comments: {model.Comments}" : ""),
                            "Request",
                            baseRequest.Id,
                            new List<string> { baseRequest.CreatedBy },
                            null,
                            _currentUserService.UserId
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send final approval notification email to requester for request {RequestId}", baseRequest.Id);
                    }
                });
            }

            baseRequest.ModifiedBy = _currentUserService.UserId;
            baseRequest.ModificationDate = DateTime.UtcNow;
        }


        // Reject step
        private async Task RejectStepAsync(WorkflowApprovalStep step, ApproveRejectWorkflowApprovalDto model)
        {
            // Prevent double approval/rejection
            if (step.Status == RequestStatus.Approved || step.Status == RequestStatus.Rejected)
            {
                throw new Exception("This step has already been processed.");
            }

            // Only current step can be processed
            if (!step.IsCurrent)
            {
                throw new Exception("This step is not active anymore.");
            }

            step.Status = RequestStatus.Rejected;
            step.ApproverUserId = _currentUserService.UserId;
            step.ApprovedDate = DateTime.UtcNow;
            step.IsCurrent = false;
            step.ModifiedBy = _currentUserService.UserId;
            step.ModificationDate = DateTime.UtcNow;

            var baseRequest = await _context.BaseRequests.FirstOrDefaultAsync(x => x.Id == step.TargetRequestId);
            if (baseRequest == null) return;

            // Close other current steps
            var otherSteps = await _context.WorkflowApprovalSteps
                .Where(x => x.TargetRequestId == step.TargetRequestId &&
                            x.IsCurrent &&
                            (x.Status == RequestStatus.New || x.Status == RequestStatus.UnderProcess) &&
                            x.Id != step.Id)
                .ToListAsync();

            foreach (var s in otherSteps)
            {
                s.IsCurrent = false;
                s.ModifiedBy = _currentUserService.UserId;
                s.ModificationDate = DateTime.UtcNow;
            }

            baseRequest.Status = RequestStatus.Rejected;

            // Get rejector info for email notifications
            var rejector = await _context.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);
            var rejectorName = rejector?.FullNameEN ?? rejector?.FullNameAR ?? rejector?.UserName ?? "Approver";
            var stepNumber = step.WorkflowStep?.StepOrder ?? 0;
            var requestDepartmentId = baseRequest.DepartmentId > 0 ? (long?)baseRequest.DepartmentId : null;

            // 📧 Send email to requester about rejection (fire and forget - don't block)
            // Requester always receives email regardless of department
            _ = Task.Run(async () =>
            {
                try
                {
                    await _notificationHelperService.SendNotificationAndEmailAsync(
                        $"Request {baseRequest.RequestNo} - Rejected",
                        $"Your {baseRequest.RequestType} request {baseRequest.RequestNo} has been rejected at step {stepNumber} by {rejectorName}." +
                        (!string.IsNullOrWhiteSpace(model.Comments) ? $" Reason: {model.Comments}" : ""),
                        "Request",
                        baseRequest.Id,
                        new List<string> { baseRequest.CreatedBy },
                        null,
                        _currentUserService.UserId
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send rejection notification email to requester for request {RequestId}", baseRequest.Id);
                }
            });

            // 📧 Send confirmation email to current rejector (fire and forget - don't block)
            // Only send if department matches for restricted roles, otherwise send normally
            _ = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation(
                        "Checking if rejection confirmation email should be sent to rejector {RejectorId} for request {RequestId}, DepartmentId: {DeptId}",
                        _currentUserService.UserId, baseRequest.Id, requestDepartmentId);

                    bool shouldSendEmail = await ShouldSendEmailToUserAsync(_currentUserService.UserId, requestDepartmentId);
                    
                    _logger.LogInformation(
                        "ShouldSendEmailToUserAsync returned {ShouldSend} for rejector {RejectorId}",
                        shouldSendEmail, _currentUserService.UserId);

                    if (shouldSendEmail)
                    {
                        _logger.LogInformation(
                            "Sending rejection confirmation email to rejector {RejectorId} for request {RequestId}",
                            _currentUserService.UserId, baseRequest.Id);

                        await _notificationHelperService.SendNotificationAndEmailAsync(
                            $"Rejection Confirmation - Request {baseRequest.RequestNo}",
                            $"You have successfully rejected {baseRequest.RequestType} request {baseRequest.RequestNo} at step {stepNumber}." +
                            (!string.IsNullOrWhiteSpace(model.Comments) ? $" Your comments: {model.Comments}" : ""),
                            "Request",
                            baseRequest.Id,
                            new List<string> { _currentUserService.UserId },
                            null,
                            _currentUserService.UserId
                        );

                        _logger.LogInformation(
                            "Rejection confirmation email sent successfully to rejector {RejectorId} for request {RequestId}",
                            _currentUserService.UserId, baseRequest.Id);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Skipping rejection confirmation email to rejector {RejectorId} - department mismatch for restricted role or other condition",
                            _currentUserService.UserId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, 
                        "Failed to send rejection confirmation email to rejector {RejectorId} for request {RequestId}. Error: {ErrorMessage}",
                        _currentUserService.UserId, baseRequest.Id, ex.Message);
                }
            });

            baseRequest.ModifiedBy = _currentUserService.UserId;
            baseRequest.ModificationDate = DateTime.UtcNow;
        }

        // Return for review
       
        // Helper: get current approval step by request ID
        public async Task<WorkflowApprovalStep> GetCurrentApprovalStepByRequestIdAsync(int requestId)
        {
            var step = await _context.WorkflowApprovalSteps
                .Include(x => x.WorkflowStep)
                .FirstOrDefaultAsync(x => x.TargetRequestId == requestId && x.IsCurrent);

            if (step == null)
                return null;

            // Permission check
            var workflowStep = step.WorkflowStep;
            var userRoles = await _context.Set<IdentityUserRole<string>>()
                .Where(ur => ur.UserId == _currentUserService.UserId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var allowedRoles = new List<string> { workflowStep.ApplicationRoleId };
            if (!string.IsNullOrEmpty(workflowStep.HigherApprovalRoleId))
                allowedRoles.Add(workflowStep.HigherApprovalRoleId);

            if (!_currentUserService.IsSuperAdmin && !userRoles.Any(r => allowedRoles.Contains(r)))
                throw new UnauthorizedAccessException("User cannot approve/reject this step");

            return step;
        }

        // Helper: log step action
        private async Task LogStepActionAsync(int stepId, int? workflowStepId, RequestStatus oldStatus, RequestStatus newStatus, string comments, string changedBy)
        {
            _context.WorkflowStepApprovalLog.Add(new WorkflowStepApprovalLog
            {
                WorkflowApprovalStepId = stepId,
                WorkflowStepId = workflowStepId,
                OldRequestStatus = oldStatus,
                NewRequestStatus = newStatus,
                Comments = comments,
                ChangedBy = changedBy,
                ChangedAt = DateTime.UtcNow,
                CreatedBy = changedBy,
                CreationDate = DateTime.UtcNow,
                ModifiedBy = changedBy,
                ModificationDate = DateTime.UtcNow
            });

            await Task.CompletedTask;
        }
        private async Task<bool> HandleHigherApprovalAsync(WorkflowApprovalStep step, BaseRequest baseRequest)
        {
            // If not configured for higher approval → skip
            if (!step.WorkflowStep.RequireHigherApproval ||
                string.IsNullOrEmpty(step.WorkflowStep.HigherApprovalRoleId))
                return false;

            // Create new higher approval step
            var higherApproval = new WorkflowApprovalStep
            {
                WorkflowStepId = step.WorkflowStepId,
                TargetRequestId = step.TargetRequestId,
                RequestType = step.RequestType,
                Status = RequestStatus.New,
                IsCurrent = true,
                CreatedBy = _currentUserService.UserId,
                CreationDate = DateTime.UtcNow
            };

            _context.WorkflowApprovalSteps.Add(higherApproval);

            baseRequest.Status = RequestStatus.UnderProcess;

            // Send notification to higher approvers
            // Filter by department for restricted roles, otherwise send to all users in role
            var requestDepartmentId = baseRequest.DepartmentId > 0 ? (long?)baseRequest.DepartmentId : null;
            var higherApprovalRoleIds = new List<string> { step.WorkflowStep.HigherApprovalRoleId };

            // Separate restricted and non-restricted roles
            var (restrictedRoleIds, nonRestrictedRoleIds) = await SeparateRestrictedRolesAsync(higherApprovalRoleIds);

            // For restricted roles: get filtered user IDs by department
            var restrictedUserIds = new List<string>();
            if (restrictedRoleIds.Any() && requestDepartmentId.HasValue)
            {
                restrictedUserIds = await GetFilteredApproverUserIdsAsync(restrictedRoleIds, requestDepartmentId);
            }

            // Combine restricted user IDs and non-restricted role IDs
            if (restrictedUserIds.Any() || nonRestrictedRoleIds.Any())
            {
                await _notificationHelperService.SendNotificationAndEmailAsync(
                    "Higher Approval Required",
                    $"A request {baseRequest.RequestNo} requires higher approval.",
                    "Request",
                    baseRequest.Id,
                    restrictedUserIds.Any() ? restrictedUserIds : null,
                    nonRestrictedRoleIds.Any() ? nonRestrictedRoleIds : null,
                    _currentUserService.UserId
                );
            }
            else
            {
                _logger.LogInformation(
                    "No higher approvers found (after department filtering) for request {RequestId}",
                    baseRequest.Id);
            }

            return true;
        }

        /// <summary>
        /// Send notifications to all configured notifiers for a workflow step when an action is taken
        /// </summary>
        private async Task SendNotificationsToStepNotifiersAsync(int workflowStepId, ApproveRejectWorkflowApprovalDto model)
        {
            try
            {
                // Get notifiers for this step
                var notifiersResult = await _workflowStepNotifierService.GetNotifierIdsByStepIdAsync(workflowStepId);
                
                if (!notifiersResult.Succeeded || 
                    (notifiersResult.Data.UserIds.Count == 0 && notifiersResult.Data.RoleIds.Count == 0))
                {
                    // No notifiers configured for this step - this is fine, just return
                    return;
                }

                var (userIds, roleIds) = notifiersResult.Data;

                // Get the base request for context
                var baseRequest = await _context.BaseRequests
                    .FirstOrDefaultAsync(x => x.Id == model.BaseRequestID);

                if (baseRequest == null)
                {
                    return;
                }

                // Get the approver name for the message
                var approver = await _context.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);
                var approverName = approver?.FullNameEN ?? approver?.FullNameAR ?? approver?.UserName ?? "System";

                // Determine action message and title
                var (title, message) = model.Action switch
                {
                    RequestStatus.Approved => (
                        $"Request #{baseRequest.RequestNo} Approved",
                        $"Request #{baseRequest.RequestNo} has been approved by {approverName}" +
                        (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}")
                    ),
                    RequestStatus.Rejected => (
                        $"Request #{baseRequest.RequestNo} Rejected",
                        $"Request #{baseRequest.RequestNo} has been rejected by {approverName}" +
                        (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}")
                    ),
                    //RequestStatus.Returned => (
                    //    "Request {baseRequest.RequestNo} Returned",
                    //    $"Request #{baseRequest.RequestNo} has been returned by {approverName}" +
                    //    (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}")
                    //),
                    _ => (
                        $"Request #{baseRequest.RequestNo} Action Taken",
                        $"An action has been taken on request #{baseRequest.RequestNo} by {approverName}"
                    )
                };

                // Send notification to all notifiers
                await _notificationHelperService.SendNotificationAsync(
                    title: title,
                    message: message,
                    entityType: "Request",
                    entityId: baseRequest.Id,
                    userIds: userIds.Any() ? userIds : null,
                    roleIds: roleIds.Any() ? roleIds : null,
                    senderId: _currentUserService.UserId
                );
            }
            catch (Exception ex)
            {
                // Log error but don't fail the workflow action - notifications are non-critical
                _logger.LogError(ex,
                    "Error sending notifications to step notifiers for workflow step {WorkflowStepId}, RequestId: {RequestId}",
                    workflowStepId,
                    model.BaseRequestID);
            }
        }

        /// <summary>
        /// Send notifications to step notifiers when a workflow starts (first step becomes active)
        /// </summary>
        private async Task SendNotificationsToStepNotifiersOnWorkflowStartAsync(int workflowStepId, long requestId)
        {
            try
            {
                // Get notifiers for this step
                var notifiersResult = await _workflowStepNotifierService.GetNotifierIdsByStepIdAsync(workflowStepId);
                
                if (!notifiersResult.Succeeded || 
                    (notifiersResult.Data.UserIds.Count == 0 && notifiersResult.Data.RoleIds.Count == 0))
                {
                    // No notifiers configured for this step - this is fine, just return
                    return;
                }

                var (userIds, roleIds) = notifiersResult.Data;

                // Get the base request for context
                var baseRequest = await _context.BaseRequests
                    .FirstOrDefaultAsync(x => x.Id == requestId);

                if (baseRequest == null)
                {
                    return;
                }

                // Send notification to all notifiers
                await _notificationHelperService.SendNotificationAsync(
                    title: "Request Management",
                    message: $"Request #{baseRequest.RequestNo} has reached a workflow step that requires your attention.",
                    entityType: "Request",
                    entityId: baseRequest.Id,
                    userIds: userIds.Any() ? userIds : null,
                    roleIds: roleIds.Any() ? roleIds : null,
                    senderId: _currentUserService.UserId
                );
            }
            catch (Exception ex)
            {
                // Log error but don't fail workflow start - notifications are non-critical
                _logger.LogError(ex,
                    "Error sending start notifications to step notifiers for workflow step {WorkflowStepId}, RequestId: {RequestId}",
                    workflowStepId,
                    requestId);
            }
        }

        /// <summary>
        /// Start workflow for a newly created order
        /// Gets the appropriate workflow based on workflow type and creates the first approval step
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="workflowType">The workflow type to use (Order or OrderFromReAl)</param>
        /// <returns>True if workflow was started successfully, false otherwise</returns>
        public async Task<bool> StartWorkflowAsync(long orderId, WorkflowType workflowType)
        {
            try
            {
                // Get workflow after creating order
                var workflow = await _context.Workflows
                    .Include(w => w.WorkflowSteps)
                    .FirstOrDefaultAsync(w => w.IsActive && !w.IsDeleted && w.WorkflowType == workflowType);

                if (workflow != null)
                {
                    // Create a new row in WorkflowApprovalSteps
                    if (workflow.WorkflowSteps != null && workflow.WorkflowSteps.Any())
                    {
                        // Get the first workflow step (lowest StepOrder)
                        var firstWorkflowStep = workflow.WorkflowSteps
                            .OrderBy(ws => ws.StepOrder)
                            .FirstOrDefault();

                        if (firstWorkflowStep != null)
                        {
                            var workflowApprovalStep = new WorkflowApprovalStep
                            {
                                WorkflowStepId = firstWorkflowStep.Id,
                                TargetRequestId = (int)orderId,
                                RequestType = workflowType,
                                Status = RequestStatus.New,
                                IsCurrent = true,
                                CreationDate = DateTime.UtcNow,
                                CreatedBy = _currentUserService.UserId
                            };

                            _context.WorkflowApprovalSteps.Add(workflowApprovalStep);
                            await _context.SaveChangesAsync();

                            // Get the base request to check department ID
                            var baseRequest = await _context.BaseRequests.FirstOrDefaultAsync(br => br.Id == orderId);
                            if (baseRequest == null)
                            {
                                _logger.LogWarning("Base request not found for orderId {OrderId} when starting workflow", orderId);
                                return false;
                            }

                            var requestDepartmentId = baseRequest.DepartmentId > 0 ? (long?)baseRequest.DepartmentId : null;

                            // Notify the approver roles after creating the workflow approval step
                            // Filter by department for restricted roles, otherwise send to all users in role
                            var approverRoles = new List<string> { firstWorkflowStep.ApplicationRoleId };
                            if (!string.IsNullOrEmpty(firstWorkflowStep.HigherApprovalRoleId))
                                approverRoles.Add(firstWorkflowStep.HigherApprovalRoleId);

                            // Separate restricted and non-restricted roles
                            var (restrictedRoleIds, nonRestrictedRoleIds) = await SeparateRestrictedRolesAsync(approverRoles);

                            // For restricted roles: get filtered user IDs by department
                            var restrictedUserIds = new List<string>();
                            if (restrictedRoleIds.Any() && requestDepartmentId.HasValue)
                            {
                                restrictedUserIds = await GetFilteredApproverUserIdsAsync(restrictedRoleIds, requestDepartmentId);
                            }

                            // Combine restricted user IDs and non-restricted role IDs
                            if (restrictedUserIds.Any() || nonRestrictedRoleIds.Any())
                            {
                                await _notificationHelperService.SendNotificationAndEmailAsync(
                                    "New Approval Required",
                                    $"A request {baseRequest.RequestNo} awaits your approval.",
                                    "Request",
                                    orderId,
                                    restrictedUserIds.Any() ? restrictedUserIds : null,
                                    nonRestrictedRoleIds.Any() ? nonRestrictedRoleIds : null,
                                    _currentUserService.UserId
                                );
                            }
                            else
                            {
                                _logger.LogInformation(
                                    "No approvers found (after department filtering) for request {RequestId} when starting workflow",
                                    orderId);
                            }

                            // 🔔 Also notify step notifiers if configured
                            await SendNotificationsToStepNotifiersOnWorkflowStartAsync(firstWorkflowStep.Id, orderId);

                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                // Don't throw - allow order creation to succeed even if workflow initialization fails
                return false;
            }
        }

        public async Task<IEnumerable<BaseRequestDto>> GetAllBaseRequestsAsync()
        {
            var currentUserId = _currentUserService.UserId;
            
            // Get current user's department ID
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == currentUserId);
            
            var currentUserDepartmentId = currentUser?.DepartmentId;
            List<long> allowedRequestIds;

            // Get all role IDs and role names of current user
            var userRoles = await (from ur in _context.Set<IdentityUserRole<string>>()
                                  join r in _context.Roles on ur.RoleId equals r.Id
                                  where ur.UserId == currentUserId
                                  select new { RoleId = ur.RoleId, RoleName = r.Name })
                                  .ToListAsync();

            var userRoleIds = userRoles.Select(r => r.RoleId).ToList();
            var userRoleNames = userRoles.Select(r => r.RoleName).ToList();

            // Check if user has roles that require department matching
            var requiresDepartmentCheck = userRoleNames.Any(rn => 
                rn == "Supply Officer (Order Requesting Entity)" || 
                rn == "Requesting Entity Commander (Order Requesting Entity)");

            // If superadmin, get all request IDs
            if (_currentUserService.IsSuperAdmin)
            {
                // Only filter by department if superadmin has one of the specific roles
                if (requiresDepartmentCheck)
                {
                    if (!currentUserDepartmentId.HasValue)
                    {
                        return Enumerable.Empty<BaseRequestDto>();
                    }
                    
                    allowedRequestIds = await _context.BaseRequests
                        .Where(br => !br.IsDeleted && br.DepartmentId == currentUserDepartmentId.Value)
                        .Select(br => br.Id)
                        .ToListAsync();
                }
                else
                {
                    // Superadmin without those roles - get all requests
                    allowedRequestIds = await _context.BaseRequests
                        .Where(br => !br.IsDeleted)
                        .Select(br => br.Id)
                        .ToListAsync();
                }
            }
            else
            {
                if (!userRoleIds.Any())
                    return Enumerable.Empty<BaseRequestDto>();

                // Get request IDs that the user has permission to approve
                var baseQuery = from ws in _context.WorkflowApprovalSteps
                            join br in _context.BaseRequests
                                on (long)ws.TargetRequestId equals br.Id
                            join wfs in _context.WorkflowSteps
                                on ws.WorkflowStepId equals wfs.Id
                            where
                                !br.IsDeleted &&
                                (
                                    // Step assigned directly to this user
                                    ws.ApproverUserId == currentUserId
                                    // OR user role matches main approver
                                    || userRoleIds.Contains(wfs.ApplicationRoleId)
                                    // OR user role matches higher approval
                                    || (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId) && userRoleIds.Contains(wfs.HigherApprovalRoleId))
                                )
                            select new { RequestId = br.Id, DepartmentId = br.DepartmentId };
                
                // Apply department filter only if user has one of the specific roles
                if (requiresDepartmentCheck)
                {
                    if (!currentUserDepartmentId.HasValue)
                    {
                        return Enumerable.Empty<BaseRequestDto>();
                    }
                    
                    // Filter by matching department IDs: requester's department must match approver's (current user's) department
                    allowedRequestIds = await baseQuery
                        .Where(x => x.DepartmentId == currentUserDepartmentId.Value)
                        .Select(x => x.RequestId)
                        .Distinct()
                        .ToListAsync();
                }
                else
                {
                    // No department filter - get all requests user can approve
                    allowedRequestIds = await baseQuery
                        .Select(x => x.RequestId)
                        .Distinct()
                        .ToListAsync();
                }
            }

            if (!allowedRequestIds.Any())
                return Enumerable.Empty<BaseRequestDto>();

            // Get BaseRequests that the user has permission to approve
            // Note: Department ID filtering is already applied in the allowedRequestIds query above
            var baseRequests = await _context.BaseRequests
                .Include(br => br.Requester)
                .Include(br => br.Department)
                .Include(br => br.RequestPurpose)
                .Where(br => !br.IsDeleted && allowedRequestIds.Contains(br.Id))
                .Select(br => new BaseRequestDto
                {
                    Id = br.Id,
                    RequestNo = br.RequestNo,
                    RequestType = br.RequestType,
                    Reason = br.Reason,
                    Priority = br.Priority,
                    Status = br.Status,
                    RequestDate = br.CreationDate,
                    Notes = br.Notes,
                    DepartmentId = br.DepartmentId,
                    RequesterId = br.RequesterId,
                    RequestPurposeId = br.RequestPurposeId,
                    DepartmentName = br.Department != null ? br.Department.NameEn : null,
                    RequesterName = br.Requester != null ? (br.Requester.FullNameEN ?? br.Requester.FullNameAR ?? br.Requester.UserName) : null,
                    RequesterUserName = br.Requester != null ? br.Requester.UserName : null,
                    RequestPurposeName = br.RequestPurpose != null ? br.RequestPurpose.NameEn : null
                })
                .ToListAsync();

            // Get all approval history for these requests
            var approvalHistoryData = await (from log in _context.WorkflowStepApprovalLog
                                            join was in _context.WorkflowApprovalSteps
                                                on log.WorkflowApprovalStepId equals was.Id
                                            join wfs in _context.WorkflowSteps
                                                on log.WorkflowStepId equals wfs.Id into wfsJoin
                                            from wfs in wfsJoin.DefaultIfEmpty()
                                            where allowedRequestIds.Contains((long)was.TargetRequestId)
                                            select new
                                            {
                                                RequestId = (long)was.TargetRequestId,
                                                History = new ApprovalHistoryDto
                                                {
                                                    Id = log.Id,
                                                    WorkflowApprovalStepId = log.WorkflowApprovalStepId,
                                                    WorkflowStepId = log.WorkflowStepId,
                                                    OldRequestStatus = log.OldRequestStatus,
                                                    NewRequestStatus = log.NewRequestStatus,
                                                    Comments = log.Comments,
                                                    ChangedBy = log.ChangedBy,
                                                    ChangedAt = log.ChangedAt,
                                                    StepOrder = wfs != null ? wfs.StepOrder : (int?)null,
                                                    ApplicationRoleId = wfs != null ? wfs.ApplicationRoleId : null,
                                                    RequireHigherApproval = wfs != null ? wfs.RequireHigherApproval : false,
                                                    HigherApprovalRoleId = wfs != null ? wfs.HigherApprovalRoleId : null
                                                }
                                            })
                                            .OrderBy(h => h.History.ChangedAt)
                                            .ToListAsync();

            // Group approval history by request ID
            var historyByRequestId = approvalHistoryData
                .GroupBy(h => h.RequestId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.History).ToList());

            // Get all active workflows with their steps, grouped by WorkflowType
            var workflowsByType = await _context.Workflows
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(ws => ws.ApplicationRole)
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(ws => ws.HigherApprovalRole)
                .Where(w => w.IsActive && !w.IsDeleted)
                .GroupBy(w => w.WorkflowType)
                .ToDictionaryAsync(g => g.Key, g => g.FirstOrDefault());

            // Get pending workflow approval steps for these requests
            var pendingStepsData = await (from was in _context.WorkflowApprovalSteps
                                         join wfs in _context.WorkflowSteps
                                             .Include(ws => ws.ApplicationRole)
                                             .Include(ws => ws.HigherApprovalRole)
                                             on was.WorkflowStepId equals wfs.Id
                                         where allowedRequestIds.Contains((long)was.TargetRequestId) &&
                                               (was.Status == RequestStatus.New || was.Status == RequestStatus.UnderProcess)
                                         select new
                                         {
                                             RequestId = (long)was.TargetRequestId,
                                             WorkflowStepId = was.WorkflowStepId,
                                             WorkflowApprovalStepId = was.Id,
                                             Status = was.Status,
                                             Comments = was.Comments,
                                             CreationDate = was.CreationDate,
                                             StepOrder = wfs.StepOrder,
                                             ApplicationRoleId = wfs.ApplicationRoleId,
                                             ApplicationRoleName = wfs.ApplicationRole != null ? wfs.ApplicationRole.Name : null,
                                             RequireHigherApproval = wfs.RequireHigherApproval,
                                             HigherApprovalRoleId = wfs.HigherApprovalRoleId,
                                             HigherApprovalRoleName = wfs.HigherApprovalRole != null ? wfs.HigherApprovalRole.Name : null
                                         })
                                         .ToListAsync();

            // Group pending steps by request ID
            var pendingStepsByRequestId = pendingStepsData
                .GroupBy(p => p.RequestId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Assign approval history and merge with all workflow steps for each request
            foreach (var request in baseRequests)
            {
                var combinedHistory = new List<ApprovalHistoryDto>();

                // Get completed approval history
                if (historyByRequestId.TryGetValue(request.Id, out var history))
                {
                    combinedHistory.AddRange(history);
                }

                // Check if request has been rejected - if so, don't show any pending/future steps
                bool isRejected = request.Status == RequestStatus.Rejected;
                
                // Also check if any step in the history has been rejected
                if (!isRejected && combinedHistory.Any(h => h.NewRequestStatus == RequestStatus.Rejected))
                {
                    isRejected = true;
                }

                // Only show pending/future steps if request is not rejected
                if (!isRejected)
                {
                    // Get the workflow for this request type
                    var workflowType = (WorkflowType)request.RequestType;
                    if (workflowsByType.TryGetValue(workflowType, out var workflow) && workflow != null)
                    {
                        // Get all workflow steps
                        var allWorkflowSteps = workflow.WorkflowSteps.OrderBy(ws => ws.StepOrder).ToList();

                        // Create a set of workflow step IDs that have been completed or are pending
                        var completedOrPendingStepIds = new HashSet<int>();
                        foreach (var h in combinedHistory)
                        {
                            if (h.WorkflowStepId.HasValue)
                                completedOrPendingStepIds.Add(h.WorkflowStepId.Value);
                        }

                        // Get pending steps for this request and add them
                        if (pendingStepsByRequestId.TryGetValue(request.Id, out var requestPendingSteps))
                        {
                            // Only add the next pending step (the one with the lowest step order that hasn't been completed)
                            var nextPendingStep = requestPendingSteps
                                .OrderBy(p => p.StepOrder)
                                .FirstOrDefault();
                            
                            if (nextPendingStep != null)
                            {
                                completedOrPendingStepIds.Add(nextPendingStep.WorkflowStepId);
                                
                                // Check if this is a higher approval step
                                // A higher approval step exists when:
                                // 1. RequireHigherApproval is true
                                // 2. There's already an approved step with the same WorkflowStepId for this request
                                bool isHigherApprovalStep = nextPendingStep.RequireHigherApproval &&
                                    combinedHistory.Any(h => 
                                        h.WorkflowStepId == nextPendingStep.WorkflowStepId && 
                                        h.NewRequestStatus == RequestStatus.Approved);
                                
                                // Use HigherApprovalRoleName if this is a higher approval step, otherwise use ApplicationRoleName
                                string roleNameToUse = isHigherApprovalStep && !string.IsNullOrEmpty(nextPendingStep.HigherApprovalRoleName)
                                    ? nextPendingStep.HigherApprovalRoleName
                                    : nextPendingStep.ApplicationRoleName;
                                
                                var pendingStep = new ApprovalHistoryDto
                                {
                                    Id = nextPendingStep.WorkflowApprovalStepId,
                                    WorkflowApprovalStepId = nextPendingStep.WorkflowApprovalStepId,
                                    WorkflowStepId = nextPendingStep.WorkflowStepId,
                                    OldRequestStatus = nextPendingStep.Status,
                                    NewRequestStatus = nextPendingStep.Status,
                                    Comments = nextPendingStep.Comments,
                                    ChangedBy = null,
                                    ChangedAt = nextPendingStep.CreationDate,
                                    StepOrder = nextPendingStep.StepOrder,
                                    ApplicationRoleId = isHigherApprovalStep ? nextPendingStep.HigherApprovalRoleId : nextPendingStep.ApplicationRoleId,
                                    ApplicationRoleName = roleNameToUse,
                                    RequireHigherApproval = nextPendingStep.RequireHigherApproval,
                                    HigherApprovalRoleId = nextPendingStep.HigherApprovalRoleId,
                                    IsPending = true
                                };
                                combinedHistory.Add(pendingStep);
                            }
                        }
                        else
                        {
                            // If no pending steps exist, find the next workflow step that should be started
                            var nextWorkflowStep = allWorkflowSteps
                                .Where(ws => !completedOrPendingStepIds.Contains(ws.Id))
                                .OrderBy(ws => ws.StepOrder)
                                .FirstOrDefault();
                            
                            if (nextWorkflowStep != null)
                            {
                                var futureStep = new ApprovalHistoryDto
                                {
                                    Id = 0, // No approval step ID yet
                                    WorkflowApprovalStepId = 0,
                                    WorkflowStepId = nextWorkflowStep.Id,
                                    OldRequestStatus = RequestStatus.New,
                                    NewRequestStatus = RequestStatus.New,
                                    Comments = null,
                                    ChangedBy = null,
                                    ChangedAt = DateTime.MinValue, // Will be sorted last
                                    StepOrder = nextWorkflowStep.StepOrder,
                                    ApplicationRoleId = nextWorkflowStep.ApplicationRoleId,
                                    ApplicationRoleName = nextWorkflowStep.ApplicationRole != null ? nextWorkflowStep.ApplicationRole.Name : null,
                                    RequireHigherApproval = nextWorkflowStep.RequireHigherApproval,
                                    HigherApprovalRoleId = nextWorkflowStep.HigherApprovalRoleId,
                                    IsPending = true
                                };
                                combinedHistory.Add(futureStep);
                            }
                        }
                    }
                }

                // Sort: completed steps first (by step order and date), then pending steps last
                request.ApprovalHistory = combinedHistory
                    .OrderBy(h => h.IsPending ? 1 : 0) // Pending steps (1) come after completed steps (0)
                    .ThenBy(h => h.StepOrder ?? int.MaxValue)
                    .ThenBy(h => h.ChangedAt == DateTime.MinValue ? DateTime.MaxValue : h.ChangedAt)                    
                    .ToList();
            }

            return baseRequests;
        }

        /// <summary>
        /// Check if a user has one of the restricted roles that require department matching
        /// Restricted roles: "Supply Officer (Order Requesting Entity)" and "Requesting Entity Commander (Order Requesting Entity)"
        /// </summary>
        private async Task<bool> HasRestrictedRoleAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            var userRoles = await (from ur in _context.Set<IdentityUserRole<string>>()
                                  join r in _context.Roles on ur.RoleId equals r.Id
                                  where ur.UserId == userId
                                  select r.Name)
                                  .ToListAsync();

            var restrictedRoleNames = new[]
            {
                "Supply Officer (Order Requesting Entity)",
                "Requesting Entity Commander (Order Requesting Entity)"
            };

            bool hasRestricted = userRoles.Any(roleName => restrictedRoleNames.Contains(roleName));
            
            if (hasRestricted)
            {
                _logger.LogInformation(
                    "User {UserId} has restricted role. User roles: {Roles}",
                    userId, string.Join(", ", userRoles));
            }

            return hasRestricted;
        }

        /// <summary>
        /// Check if email should be sent to a user based on department matching for restricted roles
        /// </summary>
        private async Task<bool> ShouldSendEmailToUserAsync(string userId, long? requestDepartmentId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("ShouldSendEmailToUserAsync: UserId is null or empty");
                return false;
            }

            // Check if user has restricted role
            bool hasRestrictedRole = await HasRestrictedRoleAsync(userId);

            // If user doesn't have restricted role, always send email
            if (!hasRestrictedRole)
            {
                _logger.LogInformation(
                    "ShouldSendEmailToUserAsync: User {UserId} does not have restricted role - will send email",
                    userId);
                return true;
            }

            _logger.LogInformation(
                "ShouldSendEmailToUserAsync: User {UserId} has restricted role - checking department match. Request DepartmentId: {RequestDeptId}",
                userId, requestDepartmentId);

            // If user has restricted role, check department match
            if (!requestDepartmentId.HasValue)
            {
                _logger.LogWarning(
                    "ShouldSendEmailToUserAsync: Request has no department ID, skipping email to restricted role user {UserId}",
                    userId);
                return false;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("ShouldSendEmailToUserAsync: User {UserId} not found in database", userId);
                return false;
            }

            if (!user.DepartmentId.HasValue)
            {
                _logger.LogWarning(
                    "ShouldSendEmailToUserAsync: User {UserId} has restricted role but no department ID, skipping email",
                    userId);
                return false;
            }

            bool departmentMatches = user.DepartmentId.Value == requestDepartmentId.Value;
            
            if (departmentMatches)
            {
                _logger.LogInformation(
                    "ShouldSendEmailToUserAsync: Department matches for user {UserId} - will send email. User DepartmentId: {UserDeptId}, Request DepartmentId: {RequestDeptId}",
                    userId, user.DepartmentId.Value, requestDepartmentId.Value);
            }
            else
            {
                _logger.LogInformation(
                    "ShouldSendEmailToUserAsync: Skipping email to user {UserId} with restricted role - department mismatch. User DepartmentId: {UserDeptId}, Request DepartmentId: {RequestDeptId}",
                    userId, user.DepartmentId.Value, requestDepartmentId.Value);
            }

            return departmentMatches;
        }

        /// <summary>
        /// Separate roles into restricted and non-restricted roles
        /// </summary>
        private async Task<(List<string> restrictedRoleIds, List<string> nonRestrictedRoleIds)> SeparateRestrictedRolesAsync(List<string> roleIds)
        {
            var restrictedRoleIds = new List<string>();
            var nonRestrictedRoleIds = new List<string>();

            var restrictedRoleNames = new[]
            {
                "Supply Officer (Order Requesting Entity)",
                "Requesting Entity Commander (Order Requesting Entity)"
            };

            foreach (var roleId in roleIds)
            {
                if (string.IsNullOrEmpty(roleId))
                    continue;

                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                if (role == null)
                    continue;

                if (restrictedRoleNames.Contains(role.Name))
                {
                    restrictedRoleIds.Add(roleId);
                }
                else
                {
                    nonRestrictedRoleIds.Add(roleId);
                }
            }

            return (restrictedRoleIds, nonRestrictedRoleIds);
        }

        /// <summary>
        /// Get filtered user IDs for approvers based on roles and department matching for restricted roles
        /// </summary>
        private async Task<List<string>> GetFilteredApproverUserIdsAsync(List<string> roleIds, long? requestDepartmentId)
        {
            var approverUserIds = new List<string>();

            _logger.LogInformation(
                "GetFilteredApproverUserIdsAsync: Processing {Count} role IDs. Request DepartmentId: {RequestDeptId}",
                roleIds.Count, requestDepartmentId);

            foreach (var roleId in roleIds)
            {
                if (string.IsNullOrEmpty(roleId))
                {
                    _logger.LogWarning("GetFilteredApproverUserIdsAsync: Empty role ID found, skipping");
                    continue;
                }

                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                if (role == null)
                {
                    _logger.LogWarning("GetFilteredApproverUserIdsAsync: Role {RoleId} not found in database", roleId);
                    continue;
                }

                // Get all users in this role
                var usersInRole = await _context.Set<IdentityUserRole<string>>()
                    .Where(ur => ur.RoleId == roleId)
                    .Select(ur => ur.UserId)
                    .ToListAsync();

                _logger.LogInformation(
                    "GetFilteredApproverUserIdsAsync: Role {RoleName} ({RoleId}) has {UserCount} users",
                    role.Name, roleId, usersInRole.Count);

                // Check if this is a restricted role
                var restrictedRoleNames = new[]
                {
                    "Supply Officer (Order Requesting Entity)",
                    "Requesting Entity Commander (Order Requesting Entity)"
                };

                bool isRestrictedRole = restrictedRoleNames.Contains(role.Name);

                if (isRestrictedRole)
                {
                    _logger.LogInformation(
                        "GetFilteredApproverUserIdsAsync: Role {RoleName} is a restricted role - will filter by department",
                        role.Name);

                    if (requestDepartmentId.HasValue)
                    {
                        // Filter by department for restricted roles
                        var usersWithMatchingDept = await _context.Users
                            .Where(u => usersInRole.Contains(u.Id) &&
                                       u.DepartmentId.HasValue &&
                                       u.DepartmentId.Value == requestDepartmentId.Value)
                            .Select(u => u.Id)
                            .ToListAsync();

                        approverUserIds.AddRange(usersWithMatchingDept);

                        _logger.LogInformation(
                            "GetFilteredApproverUserIdsAsync: Filtered approvers for restricted role {RoleName} by department {DepartmentId}. Total users in role: {TotalCount}, Users with matching department: {MatchingCount}",
                            role.Name, requestDepartmentId.Value, usersInRole.Count, usersWithMatchingDept.Count);

                        // Log details about users that were filtered out
                        if (usersInRole.Count > usersWithMatchingDept.Count)
                        {
                            var filteredOutUsers = await _context.Users
                                .Where(u => usersInRole.Contains(u.Id) && 
                                           (!u.DepartmentId.HasValue || u.DepartmentId.Value != requestDepartmentId.Value))
                                .Select(u => new { u.Id, u.UserName, u.DepartmentId })
                                .ToListAsync();

                            foreach (var user in filteredOutUsers)
                            {
                                _logger.LogInformation(
                                    "GetFilteredApproverUserIdsAsync: User {UserId} ({UserName}) filtered out - DepartmentId: {UserDeptId}, Request DepartmentId: {RequestDeptId}",
                                    user.Id, user.UserName ?? "Unknown", user.DepartmentId, requestDepartmentId.Value);
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning(
                            "GetFilteredApproverUserIdsAsync: Request has no department ID, skipping all users in restricted role {RoleName}",
                            role.Name);
                    }
                }
                else
                {
                    // No filtering for non-restricted roles - include all users in role
                    approverUserIds.AddRange(usersInRole);
                    _logger.LogInformation(
                        "GetFilteredApproverUserIdsAsync: Role {RoleName} is not restricted - including all {Count} users",
                        role.Name, usersInRole.Count);
                }
            }

            // Remove duplicates
            var distinctUserIds = approverUserIds.Distinct().ToList();
            _logger.LogInformation(
                "GetFilteredApproverUserIdsAsync: Returning {Count} unique user IDs",
                distinctUserIds.Count);

            return distinctUserIds;
        }

    }
}
