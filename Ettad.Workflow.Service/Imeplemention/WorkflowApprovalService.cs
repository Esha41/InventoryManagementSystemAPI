using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Notification.Service;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.DTO;
using Ettad.Workflows.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Workflows.Service.Imeplemention
{
    public class WorkflowApprovalService:IWorkflowApprovalService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationHelperService _notificationHelperService;

        public WorkflowApprovalService(ApplicationDbContext context, ICurrentUserService currentUserService, INotificationHelperService notificationHelperService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _notificationHelperService = notificationHelperService;
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

                // Send notification to next step approvers
                var nextRoles = new List<string> { nextStep.ApplicationRoleId };
                if (!string.IsNullOrEmpty(nextStep.HigherApprovalRoleId))
                    nextRoles.Add(nextStep.HigherApprovalRoleId);

                await _notificationHelperService.SendNotificationAsync(
                    "New Approval Required",
                    "A request awaits your approval.",
                    "Request",
                    baseRequest.Id,
                    null,
                    nextRoles,
                    _currentUserService.UserId
                );
            }
            else
            {
                // ⭐ Final approval — now notify requester
                baseRequest.Status = RequestStatus.Approved;

                var approver = await _context.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);

                await _notificationHelperService.SendNotificationAsync(
                    "Request Approved",
                    $"Approved by {approver?.UserName}",
                    "Request",
                    baseRequest.Id,
                    new List<string> { baseRequest.CreatedBy },
                    null,
                    _currentUserService.UserId
                );
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

            // Notify requester
            var approver = await _context.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);
            await _notificationHelperService.SendNotificationAsync(
                "Request Rejected",
                $"Rejected by {approver?.UserName}",
                "Request",
                baseRequest.Id,
                new List<string> { baseRequest.CreatedBy },
                null,
                _currentUserService.UserId
            );

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

            // Send notification
            await _notificationHelperService.SendNotificationAsync(
                "Higher Approval Required",
                "A request requires higher approval.",
                "Request",
                baseRequest.Id,
                null,
                new List<string> { step.WorkflowStep.HigherApprovalRoleId },
                _currentUserService.UserId
            );

            return true;
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

                            // Notify the approver after creating the workflow approval step
                            var approverRoles = new List<string> { firstWorkflowStep.ApplicationRoleId };
                            if (!string.IsNullOrEmpty(firstWorkflowStep.HigherApprovalRoleId))
                                approverRoles.Add(firstWorkflowStep.HigherApprovalRoleId);

                            await _notificationHelperService.SendNotificationAsync(
                                "New Approval Required",
                                "A request awaits your approval.",
                                "Request",
                                orderId,
                                null,
                                approverRoles,
                                _currentUserService.UserId
                            );

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


    }
}
