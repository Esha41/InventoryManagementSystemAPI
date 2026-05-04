using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Ettad.Workflow.Service.Interface;
using Ettad.Workflows.Service.Events;
using Ettad.Workflows.Service.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Notification.Service.Interfaces;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Services;
using Ettad.Data.Constants;
using Ettad.Workflows.Service.Dtos;

namespace Ettad.Workflows.Service.Services
{
    public class WorkflowApprovalService:IWorkflowApprovalService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly IWorkflowStepNotifierService _workflowStepNotifierService;
        private readonly IUserDelegationService _userDelegationService;
        private readonly ILogger<WorkflowApprovalService> _logger;
        private readonly IMediator _mediator;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICrossCuttingRepository<FileUplodDetails> _fileDetailsRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IOrderItemTrackingService _orderItemTrackingService;
        private readonly IEffectiveRoleRepository _effectiveRoleService;
        private readonly ITransactionManager _transactionManager;

        public WorkflowApprovalService(
            ApplicationDbContext context, 
            ICurrentUserService currentUserService, 
            INotificationHelperService notificationHelperService,
            IWorkflowStepNotifierService workflowStepNotifierService,
            IUserDelegationService userDelegationService,
            ILogger<WorkflowApprovalService> logger,
            IMediator mediator,
            IFileUploadService fileUploadService,
            ICrossCuttingRepository<FileUplodDetails> fileDetailsRepository,
            IDateTimeProvider dateTimeProvider,
            IOrderItemTrackingService orderItemTrackingService,
            IEffectiveRoleRepository effectiveRoleService,
            ITransactionManager transactionManager)
        {
            _context = context;
            _currentUserService = currentUserService;
            _notificationHelperService = notificationHelperService;
            _workflowStepNotifierService = workflowStepNotifierService;
            _userDelegationService = userDelegationService;
            _logger = logger;
            _mediator = mediator;
            _fileUploadService = fileUploadService;
            _fileDetailsRepository = fileDetailsRepository;
            _dateTimeProvider = dateTimeProvider;
            _orderItemTrackingService = orderItemTrackingService;
            _effectiveRoleService = effectiveRoleService;
            _transactionManager = transactionManager;
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
                    ApproverRoleId = x.ApproverRoleId,
                    IsDelegation = x.IsDelegation,
                    ApprovedDate = x.ApprovedDate,
                    Status = x.Status,
                    Comments = x.Comments,
                    IsCurrent = x.IsCurrent,
                    ReturnToStepId = x.ReturnToStepId
                }).ToListAsync();
        }

        public async Task<WorkflowApprovalStepDto> GetByIdAsync(long id)
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
                ApproverRoleId = entity.ApproverRoleId,
                IsDelegation = entity.IsDelegation,
                ApprovedDate = entity.ApprovedDate,
                Status = entity.Status,
                Comments = entity.Comments,
                IsCurrent = entity.IsCurrent,
                ReturnToStepId = entity.ReturnToStepId
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
                CreationDate = _dateTimeProvider.Now,
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
                ApproverRoleId = entity.ApproverRoleId,
                IsDelegation = entity.IsDelegation,
                ApprovedDate = entity.ApprovedDate,
                Status = entity.Status,
                Comments = entity.Comments,
                IsCurrent = entity.IsCurrent,
                ReturnToStepId = entity.ReturnToStepId,
                CreatedBy = entity.CreatedBy
            };
        }

        public async Task<WorkflowApprovalStepDto> UpdateAsync(long id, UpdateWorkflowApprovalStepDto dto)
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
            entity.ModificationDate = _dateTimeProvider.Now;
            entity.ModifiedBy = dto.ChangedBy;

            await _context.SaveChangesAsync();

            return new WorkflowApprovalStepDto
            {
                Id = entity.Id,
                WorkflowStepId = entity.WorkflowStepId,
                TargetRequestId = entity.TargetRequestId,
                RequestType = entity.RequestType,
                ApproverUserId = entity.ApproverUserId,
                ApproverRoleId = entity.ApproverRoleId,
                IsDelegation = entity.IsDelegation,
                ApprovedDate = entity.ApprovedDate,
                Status = entity.Status,
                Comments = entity.Comments,
                IsCurrent = entity.IsCurrent,
                ReturnToStepId = entity.ReturnToStepId,
                ChangedBy = entity.ModifiedBy,
            };
        }

        public async Task<bool> DeleteAsync(long id)
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

            // 1. Effective role ID(s) for current user (active session)
            var userRoleIds = string.IsNullOrEmpty(currentUserId)
                ? new List<string>()
                : (await _effectiveRoleService.GetEffectiveRoleIdsAsync(currentUserId)).ToList();

            // 1.5 Get active delegations (users who delegated to current user for workflow approval)
            var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(currentUserId, DelegationScope.WorkflowApproval);
            var delegatorRoleIds = new List<string>();
            
            if (activeDelegatorIds != null && activeDelegatorIds.Any())
            {
                foreach (var delegatorId in activeDelegatorIds)
                    delegatorRoleIds.AddRange(await _effectiveRoleService.GetEffectiveRoleIdsAsync(delegatorId));
            }

            // Ensure lists are not null for Contains queries (though EF handles this, it's safer)
            if (userRoleIds == null) userRoleIds = new List<string>();
            if (delegatorRoleIds == null) delegatorRoleIds = new List<string>();

            // 2. Query workflow approval steps joined with workflow steps and base requests
            var query = from ws in _context.WorkflowApprovalSteps
                        join br in _context.BaseRequests
                            on ws.TargetRequestId equals br.Id
                        join wfs in _context.WorkflowSteps
                            on ws.WorkflowStepId equals wfs.Id
                        where
                            ws.IsCurrent // Only fetch current active steps
                            && (
                                // Step assigned directly to this user
                                ws.ApproverUserId == currentUserId
                                // OR user role matches main approver
                                || userRoleIds.Contains(wfs.ApplicationRoleId)
                                // OR user role matches higher approval
                                || (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId) && userRoleIds.Contains(wfs.HigherApprovalRoleId))
                                // OR user role matches a parallel approver role on this step
                                || _context.WorkflowStepParallelRoles.Any(pr => pr.WorkflowStepId == wfs.Id && userRoleIds.Contains(pr.RoleId))
                                
                                // --- Delegation Logic ---
                                // OR step assigned to a delegator
                                || activeDelegatorIds.Contains(ws.ApproverUserId)
                                // OR delegator matches main approver role
                                || delegatorRoleIds.Contains(wfs.ApplicationRoleId)
                                // OR delegator matches higher approval role
                                || (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId) && delegatorRoleIds.Contains(wfs.HigherApprovalRoleId))
                                // OR delegator matches a parallel approver role
                                || _context.WorkflowStepParallelRoles.Any(pr => pr.WorkflowStepId == wfs.Id && delegatorRoleIds.Contains(pr.RoleId))
                            )

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
                            IsDelegation = ws.IsDelegation,

                            WorkflowApprovalStepId = ws.Id
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<APIOperationResponse<bool>> ProcessActionAsync(ApproveRejectWorkflowApprovalDto model)
        {
            return await ProcessActionAsync(model, null);
        }

        public async Task<APIOperationResponse<bool>> ProcessActionAsync(ApproveRejectWorkflowApprovalDto model, List<IFormFile> files)
        {
            var ownsTransaction = !_transactionManager.HasActiveTransaction;
            if (ownsTransaction)
            {
                await _transactionManager.BeginAsync();
            }

            try
            {
                List<long> savedFileMasterIds = null;

                // Step 1: Save files first (before updating status) if files are provided
                if (files != null && files.Count > 0)
                {
                    var saveFilesResult = await _fileUploadService.SaveFilesAsync(files, FileEntityType.WorkflowApproval);
                    if (!saveFilesResult.Succeeded || saveFilesResult.Data == null)
                    {
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
                            saveFilesResult.Message ?? "Failed to save files before processing approval/rejection.");
                    }
                    savedFileMasterIds = saveFilesResult.Data;
                }

                // Get current step
                var currentStep = await GetCurrentApprovalStepByRequestIdAsync(model.BaseRequestID);
                if (currentStep == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "No current workflow step found for this request.");

                // Store original status before modification
                var oldStatus = currentStep.Status;

                // Set comments
                currentStep.Comments = model.Comments;

                var actingUserId = _currentUserService.UserId;
                var actingRoleId = await ResolveActingRoleIdAsync(actingUserId);

                // Call respective method
                switch (model.Action)
                {
                    case RequestStatus.Approved:
                        await ApproveStepAsync(currentStep, model, actingRoleId);
                        break;

                    case RequestStatus.Rejected:
                        await RejectStepAsync(currentStep, model, actingRoleId);
                        break;

                    case RequestStatus.ReturnedForReview:
                        await ReturnStepAsync(currentStep, model, actingRoleId);
                        break;

                    default:
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Invalid workflow action.");
                }

                // Log action with original status (user id + acting role for audit)
                await LogStepActionAsync(currentStep.Id, currentStep.WorkflowStepId, oldStatus, model.Action, model.Comments, actingUserId, actingRoleId);

                await _context.SaveChangesAsync();
                if (ownsTransaction)
                {
                    await _transactionManager.CommitAsync();
                }

                // Step 2: Link files to the approval step (after status update) if files were saved
                if (savedFileMasterIds != null && savedFileMasterIds.Count > 0)
                {
                    try
                    {
                        foreach (var masterId in savedFileMasterIds)
                        {
                            var detail = new FileUplodDetails
                            {
                                FileUplodMasterId = masterId,
                                Entity = FileEntityType.WorkflowApproval,
                                EntityId = currentStep.Id // Link to the approval step ID, not the request ID
                            };

                            await _fileDetailsRepository.AddAsync(detail);
                        }
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        // Log error but don't fail the approval/rejection
                        // Files are optional, so we continue even if linking fails
                        _logger.LogError(ex,
                            "Exception occurred while linking files to workflow approval step. StepId: {StepId}, RequestId: {RequestId}, Action: {Action}",
                            currentStep.Id,
                            model.BaseRequestID,
                            model.Action);
                    }
                }

                // 🔔 Send notifications to step notifiers (after transaction commits successfully)
                await SendNotificationsToStepNotifiersAsync(currentStep.WorkflowStepId, model);

                return APIOperationResponse<bool>.Success(true);
            }
            catch
            {
                if (ownsTransaction)
                {
                    await _transactionManager.RollbackAsync();
                }
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
            return await ApproveOrReject(dto, null);
        }

        public async Task<WorkflowApprovalStepDto> ApproveOrReject(ApproveRejectWorkflowApprovalDto dto, List<IFormFile> files)
        {
            // Validate action
            if (dto.Action != RequestStatus.Approved && 
                dto.Action != RequestStatus.Rejected && 
                dto.Action != RequestStatus.ReturnedForReview)
            {
                throw new InvalidOperationException($"Invalid action: {dto.Action}. Only 'Approved', 'Rejected', or 'ReturnedForReview' actions are allowed.");
            }

            // Get the step ID before processing
            var currentStep = await GetCurrentApprovalStepByRequestIdAsync(dto.BaseRequestID);
            if (currentStep == null)
            {
                throw new KeyNotFoundException($"No current workflow step found for request ID {dto.BaseRequestID}. The request may have already been processed or does not exist.");
            }

            var stepId = currentStep.Id;
            var actionName = dto.Action == RequestStatus.Approved 
                ? "approve" 
                : dto.Action == RequestStatus.Rejected 
                ? "reject" 
                : "return for review";

            try
            {
                var result = await ProcessActionAsync(dto, files);
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
                ApproverRoleId = entity.ApproverRoleId,
                IsDelegation = entity.IsDelegation,
                ApprovedDate = entity.ApprovedDate,
                Status = entity.Status,
                Comments = entity.Comments,
                IsCurrent = entity.IsCurrent,
                ReturnToStepId = entity.ReturnToStepId,
                ChangedBy = entity.ModifiedBy,
            };
        }

        // Approve step
        private async Task ApproveStepAsync(WorkflowApprovalStep step, ApproveRejectWorkflowApprovalDto model, string? approverRoleId)
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
            step.ApproverRoleId = approverRoleId;
            step.ApprovedDate = _dateTimeProvider.Now;
            step.IsCurrent = false;
            step.ModifiedBy = _currentUserService.UserId;
            step.ModificationDate = _dateTimeProvider.Now;

            var baseRequest = await _context.BaseRequests
                .FirstOrDefaultAsync(x => x.Id == step.TargetRequestId);

            if (baseRequest == null)
                return;

            //  Higher approval only when SendToHigherApproval = true
            if (model.SendToHigherApproval == true)
            {
                bool created = await HandleHigherApprovalAsync(step, baseRequest);

                if (created)
                {
                    // STOP here — do NOT send requester notification
                    return;
                }
            }

            //  Continue normal workflow
            var workflowSteps = await _context.WorkflowSteps
                .Include(ws => ws.ParallelRoles)
                .Where(ws => ws.WorkflowId == step.WorkflowStep.WorkflowId)
                .OrderBy(ws => ws.StepOrder)
                .ToListAsync();

            //  Determine the next step
            // If this step was returned for review, we need to progress sequentially back to the original step
            WorkflowStep nextStep = null;
            long? nextReturnToStepId = null;

            if (step.ReturnToStepId.HasValue)
            {
                // Find the target step we're returning to
                var targetStep = workflowSteps.FirstOrDefault(ws => ws.Id == step.ReturnToStepId.Value);
                
                // Find the next sequential step after the current one
                var sequentialNextStep = workflowSteps.FirstOrDefault(ws => ws.StepOrder > step.WorkflowStep.StepOrder);
                
                if (sequentialNextStep != null && targetStep != null)
                {
                    // If the next sequential step's order is less than or equal to the target step's order,
                    // we need to continue progressing sequentially
                    if (sequentialNextStep.StepOrder <= targetStep.StepOrder)
                    {
                        nextStep = sequentialNextStep;
                        
                        // If we haven't reached the target step yet, pass along the ReturnToStepId
                        if (sequentialNextStep.Id != step.ReturnToStepId.Value)
                        {
                            nextReturnToStepId = step.ReturnToStepId.Value;
                        }
                        // If this IS the target step, don't set ReturnToStepId (normal flow resumes)
                    }
                    else
                    {
                        // We've passed the target step, continue normal flow
                        nextStep = sequentialNextStep;
                    }
                }
            }
            else
            {
                // 🔹 Skip Logic Implementation 🔹
                if (step.WorkflowStep.CanSkip)
                {
                    // 1. Explicit skip request
                    if (model.NextStepId.HasValue)
                    {
                        if (step.WorkflowStep.Transitions.Any(t => t.TargetWorkflowStepId == model.NextStepId.Value))
                        {
                            nextStep = workflowSteps.FirstOrDefault(ws => ws.Id == model.NextStepId.Value);
                            if (nextStep == null) throw new InvalidOperationException("Target skip step not found in workflow definition.");
                        }
                        else
                        {
                             throw new InvalidOperationException("Invalid skip target provided.");
                        }
                    }
                    else
                    {
                        // 2. Auto-skip if single target exists
                        if (step.WorkflowStep.Transitions.Count == 1)
                        {
                            var targetId = step.WorkflowStep.Transitions.First().TargetWorkflowStepId;
                            nextStep = workflowSteps.FirstOrDefault(ws => ws.Id == targetId);
                        }
                        else
                        {
                            // 3. Default behavior
                            nextStep = workflowSteps.FirstOrDefault(ws => ws.StepOrder > step.WorkflowStep.StepOrder);
                        }
                    }
                }
                else
                {
                    // Normal flow - proceed to next step in sequence
                    nextStep = workflowSteps.FirstOrDefault(ws => ws.StepOrder > step.WorkflowStep.StepOrder);
                }
            }

            if (nextStep != null)
            {
                var nextApproval = new WorkflowApprovalStep
                {
                    WorkflowStepId = nextStep.Id,
                    TargetRequestId = step.TargetRequestId,
                    RequestType = step.RequestType,
                    Status = RequestStatus.New,
                    IsCurrent = true,
                    ReturnToStepId = nextReturnToStepId, // Pass along the ReturnToStepId if we're still progressing back
                    CreatedBy = _currentUserService.UserId,
                    CreationDate = _dateTimeProvider.Now
                };

                _context.WorkflowApprovalSteps.Add(nextApproval);
                baseRequest.Status = RequestStatus.UnderProcess;

                // Send notification to next step approvers (main + parallel + higher)
                var nextRoles = CollectApproverRoleIdsForWorkflowStep(nextStep);

                var (userIds, roleIds) = await FilterNotificationRecipientsByDepartmentAsync(nextRoles, baseRequest.DepartmentId);

                await _notificationHelperService.SendNotificationAsync(
                    "New Approval Required",
                    "A request awaits your approval.",
                    "Request",
                    baseRequest.Id,
                    userIds,
                    roleIds,
                    _currentUserService.UserId
                );

                //  Also notify next step notifiers if configured
                await SendNotificationsToStepNotifiersOnWorkflowStartAsync(nextStep.Id, baseRequest.Id);
            }
            else
            {
                //  Final approval — now notify requester
                baseRequest.Status = RequestStatus.Approved;

                // Record final approval history for orders
                if (baseRequest.RequestType == RequestType.Order)
                {
                    try
                    {
                        await _orderItemTrackingService.RecordFinalApprovalHistoryAsync(baseRequest.Id, step.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to record final approval history. OrderId: {OrderId}", baseRequest.Id);
                    }
                }

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
            baseRequest.ModificationDate = _dateTimeProvider.Now;

            // Publish event
            await _mediator.Publish(new WorkflowStepApprovedEvent
            {
                WorkflowApprovalStepId = step.Id,
                WorkflowStepId = step.WorkflowStepId,
                TargetRequestId = step.TargetRequestId,
                RequestType = baseRequest.RequestType,
                ApproverUserId = step.ApproverUserId,
                ApplicationRoleId = step.WorkflowStep?.ApplicationRoleId,
                ApplicationRoleName = step.WorkflowStep?.ApplicationRole?.Name
            });
        }


        // Reject step
        private async Task RejectStepAsync(WorkflowApprovalStep step, ApproveRejectWorkflowApprovalDto model, string? approverRoleId)
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
            step.ApproverRoleId = approverRoleId;
            step.ApprovedDate = _dateTimeProvider.Now;
            step.IsCurrent = false;
            step.ModifiedBy = _currentUserService.UserId;
            step.ModificationDate = _dateTimeProvider.Now;

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
                s.ModificationDate = _dateTimeProvider.Now;
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
            baseRequest.ModificationDate = _dateTimeProvider.Now;

            // Delete the draft supply record and its details for this order if any 
            if (baseRequest.RequestType == RequestType.Order)
            {
                var draftSupply = await _context.Supplies
                    .Include(s => s.SupplyDetails)
                    .FirstOrDefaultAsync(s => s.OrderId == baseRequest.Id && s.SubmissionStatus == SupplySubmissionStatus.Draft);

                if (draftSupply != null)
                {
                    if (draftSupply.SupplyDetails != null && draftSupply.SupplyDetails.Any())
                    {
                        _context.SupplyDetails.RemoveRange(draftSupply.SupplyDetails);
                    }
                    _context.Supplies.Remove(draftSupply);
                }
            }

        }

        // Return for review
        private async Task ReturnStepAsync(WorkflowApprovalStep step, ApproveRejectWorkflowApprovalDto model, string? approverRoleId)
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

            // Check if the current workflow step allows returning
            if (!step.WorkflowStep.CanReturn)
            {
                throw new InvalidOperationException("This workflow step does not allow returning. CanReturn is set to false.");
            }

            // Validate that ReturnToWorkflowStepId is provided
            if (!model.ReturnToWorkflowStepId.HasValue)
            {
                throw new InvalidOperationException("ReturnToWorkflowStepId must be specified for return action.");
            }

            // Get the workflow step to return to
            var returnToWorkflowStep = await _context.WorkflowSteps
                .Include(ws => ws.ParallelRoles)
                .FirstOrDefaultAsync(ws => ws.Id == model.ReturnToWorkflowStepId.Value);

            if (returnToWorkflowStep == null)
            {
                throw new KeyNotFoundException($"Workflow step with ID {model.ReturnToWorkflowStepId.Value} not found.");
            }

            // Validate that the return step is a previous step in the same workflow
            if (returnToWorkflowStep.WorkflowId != step.WorkflowStep.WorkflowId)
            {
                throw new InvalidOperationException("Cannot return to a step in a different workflow.");
            }

            if (returnToWorkflowStep.StepOrder >= step.WorkflowStep.StepOrder)
            {
                throw new InvalidOperationException("Can only return to a previous step.");
            }

            // Mark current step as returned for review
            step.Status = RequestStatus.ReturnedForReview;
            step.ApproverUserId = _currentUserService.UserId;
            step.ApproverRoleId = approverRoleId;
            step.ApprovedDate = _dateTimeProvider.Now;
            step.IsCurrent = false;
            step.ModifiedBy = _currentUserService.UserId;
            step.ModificationDate = _dateTimeProvider.Now;

            var baseRequest = await _context.BaseRequests
                .FirstOrDefaultAsync(x => x.Id == step.TargetRequestId);

            if (baseRequest == null)
                return;

            // Create a new approval step for the returned-to step
            // This step will have ReturnToStepId set to the current step's WorkflowStepId
            var returnApproval = new WorkflowApprovalStep
            {
                WorkflowStepId = returnToWorkflowStep.Id,
                TargetRequestId = step.TargetRequestId,
                RequestType = step.RequestType,
                Status = RequestStatus.New,
                IsCurrent = true,
                ReturnToStepId = step.WorkflowStepId, // When this step is approved, return to the original step
                CreatedBy = _currentUserService.UserId,
                CreationDate = _dateTimeProvider.Now
            };

            _context.WorkflowApprovalSteps.Add(returnApproval);
            baseRequest.Status = RequestStatus.ReturnedForReview;

            // Send notification to the returned-to step approvers (main + parallel + higher)
            var returnRoles = CollectApproverRoleIdsForWorkflowStep(returnToWorkflowStep);

            var (userIds, roleIds) = await FilterNotificationRecipientsByDepartmentAsync(returnRoles, baseRequest.DepartmentId);

            var approver = await _context.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);
            await _notificationHelperService.SendNotificationAsync(
                "Request Returned for Review",
                $"Request #{baseRequest.RequestNo} has been returned for review by {approver?.UserName}. Comments: {model.Comments}",
                "Request",
                baseRequest.Id,
                userIds,
                roleIds,
                _currentUserService.UserId
            );

            //  Also notify step notifiers if configured
            await SendNotificationsToStepNotifiersOnWorkflowStartAsync(returnToWorkflowStep.Id, baseRequest.Id);

            baseRequest.ModifiedBy = _currentUserService.UserId;
            baseRequest.ModificationDate = _dateTimeProvider.Now;
        }

        // Helper: get current approval step by request ID
        public async Task<WorkflowApprovalStep> GetCurrentApprovalStepByRequestIdAsync(long requestId)
        {
            var steps = await _context.WorkflowApprovalSteps
                .Include(x => x.WorkflowStep)
                    .ThenInclude(ws => ws.ApplicationRole)
                .Include(x => x.WorkflowStep)
                    .ThenInclude(ws => ws.ParallelRoles)
                .Include(x => x.WorkflowStep)
                    .ThenInclude(ws => ws.Transitions)
                .Where(x => x.TargetRequestId == requestId && x.IsCurrent)
                .OrderBy(x => x.Id)
                .ToListAsync();

            if (!steps.Any())
                return null;

            var currentUserId = _currentUserService.UserId;

            if (_currentUserService.IsSuperAdmin)
            {
                var s0 = steps.First();
                s0.IsDelegation = false;
                return s0;
            }

            foreach (var step in steps)
            {
                step.IsDelegation = false;
                if (await TryAuthorizeWorkflowApprovalStepAsync(step, currentUserId))
                    return step;
            }

            throw new UnauthorizedAccessException("User cannot approve/reject this step");
        }

        /// <summary>
        /// Filters notification recipients by department for restricted roles (Supply Officer, Commander).
        /// For restricted roles, returns userIds filtered by department. For other roles, returns roleIds.
        /// </summary>
        private async Task<(List<string>? userIds, List<string>? roleIds)> FilterNotificationRecipientsByDepartmentAsync(
            List<string> roleIds, 
            long? departmentId)
        {
            if (roleIds == null || !roleIds.Any())
                return (null, null);

            // Roles that are restricted to their own department
            var restrictedRoleNames = new List<string>
            {
                WorkflowRoleNames.SupplyOfficer,
                WorkflowRoleNames.RequestingEntityCommander
            };

            // Get role names for the provided role IDs
            var roles = await _context.Roles
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync();

            var restrictedRoleIds = new List<string>();
            var nonRestrictedRoleIds = new List<string>();
            var userIds = new List<string>();

            foreach (var role in roles)
            {
                if (restrictedRoleNames.Contains(role.Name))
                {
                    restrictedRoleIds.Add(role.Id);
                }
                else
                {
                    nonRestrictedRoleIds.Add(role.Id);
                }
            }

            // For restricted roles, get users in that role who belong to the department
            if (restrictedRoleIds.Any() && departmentId.HasValue)
            {
                var usersInRestrictedRoles = await (from userRole in _context.Set<IdentityUserRole<string>>()
                                                    join user in _context.Users on userRole.UserId equals user.Id
                                                    where restrictedRoleIds.Contains(userRole.RoleId) 
                                                          && user.DepartmentId == departmentId.Value
                                                          && !user.IsDeleted
                                                    select user.Id)
                                                    .Distinct()
                                                    .ToListAsync();

                userIds.AddRange(usersInRestrictedRoles);
            }

            // Return userIds if we have any, otherwise return roleIds for non-restricted roles
            if (userIds.Any())
            {
                return (userIds, nonRestrictedRoleIds.Any() ? nonRestrictedRoleIds : null);
            }
            else
            {
                return (null, roleIds);
            }
        }

        // Helper: log step action
        private async Task LogStepActionAsync(long stepId, long? workflowStepId, RequestStatus oldStatus, RequestStatus newStatus, string comments, string changedByUserId, string? changedByRoleId)
        {
            _context.WorkflowStepApprovalLog.Add(new WorkflowStepApprovalLog
            {
                WorkflowApprovalStepId = stepId,
                WorkflowStepId = workflowStepId,
                OldRequestStatus = oldStatus,
                NewRequestStatus = newStatus,
                Comments = comments,
                ChangedBy = changedByUserId,
                ChangedByRoleId = changedByRoleId,
                ChangedAt = _dateTimeProvider.Now,
                CreatedBy = changedByUserId,
                CreationDate = _dateTimeProvider.Now,
                ModifiedBy = changedByUserId,
                ModificationDate = _dateTimeProvider.Now
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
                CreationDate = _dateTimeProvider.Now
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
        /// Send notifications to all configured notifiers for a workflow step when an action is taken
        /// </summary>
        private async Task SendNotificationsToStepNotifiersAsync(long workflowStepId, ApproveRejectWorkflowApprovalDto model)
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
        private async Task SendNotificationsToStepNotifiersOnWorkflowStartAsync(long workflowStepId, long requestId)
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
            await using var transaction = await _transactionManager.BeginAsync();

            try
            {
                // Get workflow after creating order
                var workflow = await _context.Workflows
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.ParallelRoles)
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
                                TargetRequestId = orderId,
                                RequestType = workflowType,
                                Status = RequestStatus.New,
                                IsCurrent = true,
                                CreationDate = _dateTimeProvider.Now,
                                CreatedBy = _currentUserService.UserId
                            };

                            _context.WorkflowApprovalSteps.Add(workflowApprovalStep);
                            await _context.SaveChangesAsync();
                            await _transactionManager.CommitAsync();

                            // Get the base request to access department ID
                            var baseRequest = await _context.BaseRequests
                                .FirstOrDefaultAsync(br => br.Id == orderId);

                            // Notify the approver roles after creating the workflow approval step (main + parallel + higher)
                            var approverRoles = CollectApproverRoleIdsForWorkflowStep(firstWorkflowStep);

                            var (userIds, roleIds) = await FilterNotificationRecipientsByDepartmentAsync(
                                approverRoles, 
                                baseRequest?.DepartmentId);

                            await _notificationHelperService.SendNotificationAsync(
                                "New Approval Required",
                                "A request awaits your approval.",
                                "Request",
                                orderId,
                                userIds,
                                roleIds,
                                _currentUserService.UserId
                            );

                            // 🔔 Also notify step notifiers if configured
                            await SendNotificationsToStepNotifiersOnWorkflowStartAsync(firstWorkflowStep.Id, orderId);

                            return true;
                        }
                    }
                }

                await _transactionManager.CommitAsync();
                return false;
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackAsync();
                // Log the error but don't throw - allow order creation to succeed even if workflow initialization fails
                _logger.LogError(ex, "Error starting workflow for order. OrderId: {OrderId}, WorkflowType: {WorkflowType}",
                    orderId, workflowType);
                return false;
            }
        }

        public async Task<IEnumerable<BaseRequestDto>> GetAllBaseRequestsAsync()
        {
            var currentUserId = _currentUserService.UserId;
            var userDepartmentId = _currentUserService.DepartmentId;

            // --- DELEGATION & ROLE PRE-FETCHING START ---
            var userRoleIds = string.IsNullOrEmpty(currentUserId)
                ? new List<string>()
                : (await _effectiveRoleService.GetEffectiveRoleIdsAsync(currentUserId)).ToList();

            var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(currentUserId, DelegationScope.WorkflowApproval);
            
            var delegatorRoleIds = new List<string>();
            foreach (var delegatorId in activeDelegatorIds)
                delegatorRoleIds.AddRange(await _effectiveRoleService.GetEffectiveRoleIdsAsync(delegatorId));

            var allRelevantRoleNames = new List<string>();
            if (userRoleIds.Count > 0)
            {
                allRelevantRoleNames.AddRange(await _context.Roles
                    .Where(r => userRoleIds.Contains(r.Id))
                    .Select(r => r.Name!)
                    .ToListAsync());
            }
            foreach (var delegatorId in activeDelegatorIds)
            {
                var dRoleIds = await _effectiveRoleService.GetEffectiveRoleIdsAsync(delegatorId);
                if (dRoleIds.Count > 0)
                {
                    allRelevantRoleNames.AddRange(await _context.Roles
                        .Where(r => dRoleIds.Contains(r.Id))
                        .Select(r => r.Name!)
                        .ToListAsync());
                }
            }
            // --- DELEGATION & ROLE PRE-FETCHING END ---

            List<long> allowedRequestIds;

            // Roles that are restricted to their own department
            var restrictedRoles = new List<string>
            {
                WorkflowRoleNames.SupplyOfficer,
                WorkflowRoleNames.RequestingEntityCommander
            };

            var userRoleNames = userRoleIds.Count == 0
                ? new List<string>()
                : await _context.Roles
                    .Where(r => userRoleIds.Contains(r.Id))
                    .Select(r => r.Name!)
                    .ToListAsync();

            var shouldFilterByDepartment = userDepartmentId.HasValue && 
                userRoleNames.Any(roleName => restrictedRoles.Contains(roleName));

            // If superadmin, get all request IDs (still filter by department if user has restricted role)
            if (_currentUserService.IsSuperAdmin)
            {
                var query = _context.BaseRequests.Where(br => !br.IsDeleted);
                
                // Filter by department only if user has a restricted role
                if (shouldFilterByDepartment)
                {
                    query = query.Where(br => br.DepartmentId == userDepartmentId.Value);
                }
                
                allowedRequestIds = await query.Select(br => br.Id).ToListAsync();
            }
            else
            {
                // Get request IDs that the user has permission to approve (including delegation)
                var workflowRequestIds = new List<long>();
                
                if (userRoleIds.Any() || activeDelegatorIds.Any())
                {
                    var workflowQuery = from ws in _context.WorkflowApprovalSteps
                                       join br in _context.BaseRequests
                                           on ws.TargetRequestId equals br.Id
                                       join wfs in _context.WorkflowSteps
                                           on ws.WorkflowStepId equals wfs.Id
                                       where
                                           !br.IsDeleted &&
                                           (
                                               // Direct Assignment (User OR Delegator)
                                               (ws.ApproverUserId == currentUserId || activeDelegatorIds.Contains(ws.ApproverUserId)) ||
                                               
                                               // Role Assignment (User Role OR Delegator Role) + parallel approver roles
                                               (
                                                   (userRoleIds.Contains(wfs.ApplicationRoleId) || delegatorRoleIds.Contains(wfs.ApplicationRoleId)) ||
                                                   (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId) && (userRoleIds.Contains(wfs.HigherApprovalRoleId) || delegatorRoleIds.Contains(wfs.HigherApprovalRoleId))) ||
                                                   _context.WorkflowStepParallelRoles.Any(pr => pr.WorkflowStepId == wfs.Id && (userRoleIds.Contains(pr.RoleId) || delegatorRoleIds.Contains(pr.RoleId)))
                                               )
                                           )
                                       select br;
                    
                    // Filter by department only if user has a restricted role
                    if (shouldFilterByDepartment)
                    {
                        workflowQuery = workflowQuery.Where(br => br.DepartmentId == userDepartmentId.Value);
                    }
                    
                    workflowRequestIds = await workflowQuery
                        .Select(br => br.Id)
                        .Distinct()
                        .ToListAsync();
                }

                // Get request IDs where the user is the requester
                var requesterQuery = _context.BaseRequests.Where(br => !br.IsDeleted && br.RequesterId == currentUserId);
                
                // Filter by department only if user has a restricted role
                if (shouldFilterByDepartment)
                {
                    requesterQuery = requesterQuery.Where(br => br.DepartmentId == userDepartmentId.Value);
                }
                
                var requesterRequestIds = await requesterQuery
                    .Select(br => br.Id)
                    .ToListAsync();

                // Combine both lists and remove duplicates
                allowedRequestIds = workflowRequestIds
                    .Union(requesterRequestIds)
                    .Distinct()
                    .ToList();
            }

            if (!allowedRequestIds.Any())
                return Enumerable.Empty<BaseRequestDto>();

            // Get BaseRequests that the user has permission to approve
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
                    DepartmentNameAr = br.Department != null ? br.Department.NameAr : null,
                    DepartmentNameEn = br.Department != null ? br.Department.NameEn : null,
                    RequesterName = br.Requester != null ? (br.Requester.FullNameEN ?? br.Requester.FullNameAR ?? br.Requester.UserName) : null,
                    RequesterNameEn = br.Requester != null ? br.Requester.FullNameEN : null,
                    RequesterNameAr = br.Requester != null ? br.Requester.FullNameAR : null,
                    RequesterUserName = br.Requester != null ? br.Requester.UserName : null,
                    RequestPurposeName = br.RequestPurpose != null ? br.RequestPurpose.NameEn : null,
                    RequestPurposeNameAr = br.RequestPurpose != null ? br.RequestPurpose.NameAr : null,
                    RequestPurposeNameEn = br.RequestPurpose != null ? br.RequestPurpose.NameEn : null
                })
                .ToListAsync();

            await EnrichOrderSupplyDatesAsync(baseRequests);

            // Get all approval history for these requests
            var approvalHistoryData = await (from log in _context.WorkflowStepApprovalLog
                                            join was in _context.WorkflowApprovalSteps
                                                on log.WorkflowApprovalStepId equals was.Id
                                            join wfs in _context.WorkflowSteps
                                                on log.WorkflowStepId equals wfs.Id into wfsJoin
                                            from wfs in wfsJoin.DefaultIfEmpty()
                                            join role in _context.Roles
                                                on wfs.ApplicationRoleId equals role.Id into roleJoin
                                            from role in roleJoin.DefaultIfEmpty()
                                            join actedRole in _context.Roles
                                                on log.ChangedByRoleId equals actedRole.Id into actedRoleJoin
                                            from actedRole in actedRoleJoin.DefaultIfEmpty()
                                            from user in _context.Users.Where(u => u.Id == log.ChangedBy || u.UserName == log.ChangedBy).DefaultIfEmpty()
                                            where allowedRequestIds.Contains(was.TargetRequestId)
                                            select new
                                            {
                                                RequestId = was.TargetRequestId,
                                                History = new ApprovalHistoryDto
                                                {
                                                    Id = log.Id,
                                                    WorkflowApprovalStepId = log.WorkflowApprovalStepId,
                                                    WorkflowStepId = log.WorkflowStepId,
                                                    OldRequestStatus = log.OldRequestStatus,
                                                    NewRequestStatus = log.NewRequestStatus,
                                                    Comments = log.Comments,
                                                    ChangedBy = log.ChangedBy,
                                                    ApproverNameEn = user != null ? user.FullNameEN : null,
                                                    ApproverNameAr = user != null ? user.FullNameAR : null,
                                                    ChangedAt = log.ChangedAt,
                                                    StepOrder = wfs != null ? wfs.StepOrder : null,
                                                    ApplicationRoleId = wfs != null ? wfs.ApplicationRoleId : null,
                                                    ApplicationRoleName = role != null ? role.Name : null,
                                                    ApplicationRoleNameAr = role != null ? role.NameAr : null,
                                                    RequireHigherApproval = wfs != null ? wfs.RequireHigherApproval : false,
                                                    HigherApprovalRoleId = wfs != null ? wfs.HigherApprovalRoleId : null,
                                                    CanReturn = wfs != null ? wfs.CanReturn : false,
                                                    IsDelegation = was.IsDelegation,
                                                    ChangedByRoleId = log.ChangedByRoleId,
                                                    ChangedByRoleName = actedRole != null ? actedRole.Name : null,
                                                    ChangedByRoleNameAr = actedRole != null ? actedRole.NameAr : null,
                                                    Files = new List<FileUploadDto>() // Initialize Files list
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
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(ws => ws.ParallelRoles)
                        .ThenInclude(pr => pr.Role)
                .Where(w => w.IsActive && !w.IsDeleted)
                .GroupBy(w => w.WorkflowType)
                .ToDictionaryAsync(g => g.Key, g => g.FirstOrDefault());

            // Get pending workflow approval steps for these requests
            var pendingStepsData = await (from was in _context.WorkflowApprovalSteps
                                         join wfs in _context.WorkflowSteps
                                             .Include(ws => ws.ApplicationRole)
                                             .Include(ws => ws.HigherApprovalRole)
                                             on was.WorkflowStepId equals wfs.Id
                                         where allowedRequestIds.Contains(was.TargetRequestId) &&
                                               was.IsCurrent &&
                                               (was.Status == RequestStatus.New || was.Status == RequestStatus.UnderProcess)
                                         select new
                                         {
                                             RequestId = was.TargetRequestId,
                                             was.WorkflowStepId,
                                             WorkflowApprovalStepId = was.Id,
                                             was.Status,
                                             was.Comments,
                                             was.CreationDate,
                                             wfs.StepOrder,
                                             was.ApproverUserId,
                                             wfs.ApplicationRoleId,
                                             ApplicationRoleName = wfs.ApplicationRole != null ? wfs.ApplicationRole.Name : null,
                                             ApplicationRoleNameAr = wfs.ApplicationRole != null ? wfs.ApplicationRole.NameAr : null,
                                             wfs.RequireHigherApproval,
                                             wfs.HigherApprovalRoleId,
                                             HigherApprovalRoleName = wfs.HigherApprovalRole != null ? wfs.HigherApprovalRole.Name : null,
                                             HigherApprovalRoleNameAr = wfs.HigherApprovalRole != null ? wfs.HigherApprovalRole.NameAr : null,
                                             wfs.CanReturn
                                         })
                                         .ToListAsync();

            // Group pending steps by request ID
            var pendingStepsByRequestId = pendingStepsData
                .GroupBy(p => p.RequestId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var pendingParallelWorkflowStepIds = pendingStepsData.Select(p => p.WorkflowStepId).Distinct().ToList();
            var pendingParallelRolesByWfStep = await BuildParallelRolesByStepIdsAsync(pendingParallelWorkflowStepIds);
            var pendingParallelRoleIdsByWfStep = await BuildParallelRoleIdsByStepIdsAsync(pendingParallelWorkflowStepIds);

            // Get ALL workflow approval steps for these requests (not just pending or logged ones)
            // This ensures we get files for all steps, including completed ones that might not be in the log
            var allWorkflowApprovalSteps = await _context.WorkflowApprovalSteps
                .Where(was => allowedRequestIds.Contains(was.TargetRequestId))
                .Select(was => new { was.Id, was.TargetRequestId })
                .ToListAsync();

            // Collect all workflow approval step IDs (from all steps, history, and pending steps)
            var allApprovalStepIds = new HashSet<long>();
            
            // Add all workflow approval step IDs
            foreach (var step in allWorkflowApprovalSteps)
            {
                if (step.Id > 0)
                {
                    allApprovalStepIds.Add(step.Id);
                }
            }
            
            // Also add from history (in case there are any missing)
            foreach (var historyItem in approvalHistoryData)
            {
                if (historyItem.History.WorkflowApprovalStepId > 0)
                {
                    allApprovalStepIds.Add(historyItem.History.WorkflowApprovalStepId);
                }
            }
            
            // Also add from pending steps (in case there are any missing)
            foreach (var pendingStep in pendingStepsData)
            {
                if (pendingStep.WorkflowApprovalStepId > 0)
                {
                    allApprovalStepIds.Add(pendingStep.WorkflowApprovalStepId);
                }
            }

            // Get files for all approval steps
            var filesByStepId = new Dictionary<long, List<FileUploadDto>>();
            foreach (var stepId in allApprovalStepIds)
            {
                var filesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.WorkflowApproval, (long)stepId);
                if (filesResult.Succeeded && filesResult.Data != null)
                {
                    filesByStepId[stepId] = filesResult.Data;
                }
                else
                {
                    filesByStepId[stepId] = new List<FileUploadDto>();
                }
            }

            // Get files for orders, returns, and discards
            var requestFilesByRequestId = new Dictionary<long, List<FileUploadDto>>();
            var requestIds = baseRequests
                .Where(r => r.RequestType == RequestType.Order || r.RequestType == RequestType.Return || r.RequestType == RequestType.Discard)
                .Select(r => new { r.Id, r.RequestType })
                .ToList();

            foreach (var requestInfo in requestIds)
            {
                try
                {
                   

                    var filesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Order, requestInfo.Id);
                    if (filesResult.Succeeded && filesResult.Data != null)
                    {
                        requestFilesByRequestId[requestInfo.Id] = filesResult.Data;
                    }
                    else
                    {
                        requestFilesByRequestId[requestInfo.Id] = new List<FileUploadDto>();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting files for request. RequestId: {RequestId}, RequestType: {RequestType}", 
                        requestInfo.Id, requestInfo.RequestType);
                    requestFilesByRequestId[requestInfo.Id] = new List<FileUploadDto>();
                }
            }

            // Assign approval history and merge with all workflow steps for each request
            foreach (var request in baseRequests)
            {
                var combinedHistory = new List<ApprovalHistoryDto>();

                // Assign files to order, return, and discard requests
                if ((request.RequestType == RequestType.Order || 
                     request.RequestType == RequestType.Return || 
                     request.RequestType == RequestType.Discard) &&
                    requestFilesByRequestId.TryGetValue(request.Id, out var requestFiles))
                {
                    request.Files = requestFiles;
                }
                else
                {
                    request.Files = new List<FileUploadDto>();
                }

                // Get completed approval history and assign files to each step
                if (historyByRequestId.TryGetValue(request.Id, out var history))
                {
                    foreach (var historyItem in history)
                    {
                        // Assign files to this approval step
                        if (historyItem.WorkflowApprovalStepId > 0 && filesByStepId.TryGetValue(historyItem.WorkflowApprovalStepId, out var stepFiles))
                        {
                            historyItem.Files = stepFiles;
                        }
                        else
                        {
                            historyItem.Files = new List<FileUploadDto>();
                        }
                        combinedHistory.Add(historyItem);
                    }
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
                        var completedOrPendingStepIds = new HashSet<long>();
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
                                
                                string roleNameArToUse = isHigherApprovalStep && !string.IsNullOrEmpty(nextPendingStep.HigherApprovalRoleNameAr)
                                    ? nextPendingStep.HigherApprovalRoleNameAr
                                    : nextPendingStep.ApplicationRoleNameAr;
                                
                                // Calculate IsCurrentUserApprover (including delegation)
                                bool isCurrentUserApprover = false;

                                if (_currentUserService.IsSuperAdmin)
                                {
                                    isCurrentUserApprover = true;
                                }
                                else if (!string.IsNullOrEmpty(nextPendingStep.ApproverUserId))
                                {
                                    // If assigned to specific user, check direct OR delegator
                                    isCurrentUserApprover = nextPendingStep.ApproverUserId == currentUserId || 
                                                           activeDelegatorIds.Contains(nextPendingStep.ApproverUserId);
                                }
                                else
                                {
                                    // Check role requirements (User Roles OR Delegator Roles)
                                    string requiredRoleId = isHigherApprovalStep ? nextPendingStep.HigherApprovalRoleId : nextPendingStep.ApplicationRoleId;
                                    string requiredRoleName = roleNameToUse;

                                    // Check Role ID match
                                    if (!string.IsNullOrEmpty(requiredRoleId))
                                    {
                                        if (userRoleIds.Contains(requiredRoleId) || delegatorRoleIds.Contains(requiredRoleId))
                                        {
                                            isCurrentUserApprover = true;
                                        }
                                    }
                                    
                                    // Check Role Name match (if ID didn't match)
                                    if (!isCurrentUserApprover && !string.IsNullOrEmpty(requiredRoleName))
                                    {
                                         var normalizedRequired = requiredRoleName.ToLower().Replace(" ", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace("(", "").Replace(")", "");
                                         
                                         if (allRelevantRoleNames.Any(r => !string.IsNullOrEmpty(r) && r.ToLower().Replace(" ", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace("(", "").Replace(")", "") == normalizedRequired))
                                         {
                                             isCurrentUserApprover = true;
                                         }
                                    }

                                    if (!isCurrentUserApprover && !isHigherApprovalStep &&
                                        pendingParallelRoleIdsByWfStep.TryGetValue(nextPendingStep.WorkflowStepId, out var parallelRoleIdsForApprover) &&
                                        parallelRoleIdsForApprover.Any(pid =>
                                            userRoleIds.Contains(pid) || delegatorRoleIds.Contains(pid)))
                                    {
                                        isCurrentUserApprover = true;
                                    }
                                }

                                pendingParallelRolesByWfStep.TryGetValue(nextPendingStep.WorkflowStepId, out var parallelRolesForPending);
                                parallelRolesForPending ??= new List<WorkflowStepParallelRoleDto>();

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
                                    ApplicationRoleNameAr = roleNameArToUse,
                                    RequireHigherApproval = nextPendingStep.RequireHigherApproval,
                                    HigherApprovalRoleId = nextPendingStep.HigherApprovalRoleId,
                                    CanReturn = nextPendingStep.CanReturn,
                                    IsPending = true,
                                    IsCurrentUserApprover = isCurrentUserApprover,
                                    EligibleParallelRoles = parallelRolesForPending,
                                    EligibleParallelRoleNamesEn = string.Join(" | ", parallelRolesForPending.Select(x => x.RoleName).Where(x => !string.IsNullOrEmpty(x)).Distinct()),
                                    EligibleParallelRoleNamesAr = string.Join(" | ", parallelRolesForPending.Select(x => x.RoleNameAr).Where(x => !string.IsNullOrEmpty(x)).Distinct())
                                };
                                
                                // Assign files to pending step if any
                                if (nextPendingStep.WorkflowApprovalStepId > 0 && filesByStepId.TryGetValue(nextPendingStep.WorkflowApprovalStepId, out var pendingStepFiles))
                                {
                                    pendingStep.Files = pendingStepFiles;
                                }
                                else
                                {
                                    pendingStep.Files = new List<FileUploadDto>();
                                }
                                
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
                                    ApplicationRoleNameAr = nextWorkflowStep.ApplicationRole != null ? nextWorkflowStep.ApplicationRole.NameAr : null,
                                    RequireHigherApproval = nextWorkflowStep.RequireHigherApproval,
                                    HigherApprovalRoleId = nextWorkflowStep.HigherApprovalRoleId,
                                    CanReturn = nextWorkflowStep.CanReturn,
                                    IsPending = true,
                                    EligibleParallelRoles = nextWorkflowStep.ParallelRoles != null
                                        ? nextWorkflowStep.ParallelRoles.Select(pr => new WorkflowStepParallelRoleDto
                                        {
                                            Id = pr.Id,
                                            WorkflowStepId = pr.WorkflowStepId,
                                            RoleId = pr.RoleId,
                                            RoleName = pr.Role?.Name ?? pr.RoleId,
                                            RoleNameAr = pr.Role?.NameAr ?? pr.Role?.Name ?? pr.RoleId
                                        }).ToList()
                                        : new List<WorkflowStepParallelRoleDto>(),
                                    EligibleParallelRoleNamesEn = nextWorkflowStep.ParallelRoles != null && nextWorkflowStep.ParallelRoles.Count > 0
                                        ? string.Join(" | ", nextWorkflowStep.ParallelRoles.Select(pr => pr.Role?.Name ?? pr.RoleId).Where(x => !string.IsNullOrEmpty(x)).Distinct())
                                        : null,
                                    EligibleParallelRoleNamesAr = nextWorkflowStep.ParallelRoles != null && nextWorkflowStep.ParallelRoles.Count > 0
                                        ? string.Join(" | ", nextWorkflowStep.ParallelRoles.Select(pr => pr.Role?.NameAr ?? pr.Role?.Name ?? pr.RoleId).Where(x => !string.IsNullOrEmpty(x)).Distinct())
                                        : null,
                                    Files = new List<FileUploadDto>() // No files for future steps that haven't been created yet
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

        public async Task<BaseRequestDto> GetBaseRequestByIdAsync(long requestId)
        {
            var currentUserId = _currentUserService.UserId;
            var userDepartmentId = _currentUserService.DepartmentId;

            // --- DELEGATION & ROLE PRE-FETCHING START ---
            var userRoleIds = string.IsNullOrEmpty(currentUserId)
                ? new List<string>()
                : (await _effectiveRoleService.GetEffectiveRoleIdsAsync(currentUserId)).ToList();

            var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(currentUserId, DelegationScope.WorkflowApproval);
            
            var delegatorRoleIds = new List<string>();
            foreach (var delegatorId in activeDelegatorIds)
                delegatorRoleIds.AddRange(await _effectiveRoleService.GetEffectiveRoleIdsAsync(delegatorId));

            var allRelevantRoleNames = new List<string>();
            if (userRoleIds.Count > 0)
            {
                allRelevantRoleNames.AddRange(await _context.Roles
                    .Where(r => userRoleIds.Contains(r.Id))
                    .Select(r => r.Name!)
                    .ToListAsync());
            }
            foreach (var delegatorId in activeDelegatorIds)
            {
                var dRoleIds = await _effectiveRoleService.GetEffectiveRoleIdsAsync(delegatorId);
                if (dRoleIds.Count > 0)
                {
                    allRelevantRoleNames.AddRange(await _context.Roles
                        .Where(r => dRoleIds.Contains(r.Id))
                        .Select(r => r.Name!)
                        .ToListAsync());
                }
            }
            // --- DELEGATION & ROLE PRE-FETCHING END ---

            bool hasPermission = false;

            // Roles that are restricted to their own department
            var restrictedRoles = new List<string>
            {
                WorkflowRoleNames.SupplyOfficer,
                WorkflowRoleNames.RequestingEntityCommander
            };

            var userRoleNames = userRoleIds.Count == 0
                ? new List<string>()
                : await _context.Roles
                    .Where(r => userRoleIds.Contains(r.Id))
                    .Select(r => r.Name!)
                    .ToListAsync();

            var shouldFilterByDepartment = userDepartmentId.HasValue && 
                userRoleNames.Any(roleName => restrictedRoles.Contains(roleName));

            // Check if request exists and is not deleted
            var requestExists = await _context.BaseRequests
                .AnyAsync(br => br.Id == requestId && !br.IsDeleted);
            
            if (!requestExists)
                return null;

            // If superadmin, check permission (still filter by department if user has restricted role)
            if (_currentUserService.IsSuperAdmin)
            {
                var query = _context.BaseRequests.Where(br => br.Id == requestId && !br.IsDeleted);
                
                // Filter by department only if user has a restricted role
                if (shouldFilterByDepartment)
                {
                    query = query.Where(br => br.DepartmentId == userDepartmentId.Value);
                }
                
                hasPermission = await query.AnyAsync();
            }
            else
            {
                // Check if user has permission to approve this request (including delegation)
                var hasWorkflowPermission = false;
                
                if (userRoleIds.Any() || activeDelegatorIds.Any())
                {
                    var workflowQuery = from ws in _context.WorkflowApprovalSteps
                                       join br in _context.BaseRequests
                                           on ws.TargetRequestId equals br.Id
                                       join wfs in _context.WorkflowSteps
                                           on ws.WorkflowStepId equals wfs.Id
                                       where
                                           br.Id == requestId &&
                                           !br.IsDeleted &&
                                           (
                                               // 1. Direct Assignment (User OR Delegators)
                                               (ws.ApproverUserId == currentUserId || activeDelegatorIds.Contains(ws.ApproverUserId)) ||
                                               
                                               // 2. Role Assignment (User Role OR Delegator Role) + parallel approver roles
                                               (
                                                   (userRoleIds.Contains(wfs.ApplicationRoleId) || delegatorRoleIds.Contains(wfs.ApplicationRoleId)) ||
                                                   (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId) && (userRoleIds.Contains(wfs.HigherApprovalRoleId) || delegatorRoleIds.Contains(wfs.HigherApprovalRoleId))) ||
                                                   _context.WorkflowStepParallelRoles.Any(pr => pr.WorkflowStepId == wfs.Id && (userRoleIds.Contains(pr.RoleId) || delegatorRoleIds.Contains(pr.RoleId)))
                                               )
                                           )
                                       select br;
                    
                    // Filter by department only if user has a restricted role
                    if (shouldFilterByDepartment)
                    {
                        workflowQuery = workflowQuery.Where(br => br.DepartmentId == userDepartmentId.Value);
                    }
                    
                    hasWorkflowPermission = await workflowQuery.AnyAsync();
                }

                // Check if user is the requester
                var requesterQuery = _context.BaseRequests.Where(br => br.Id == requestId && !br.IsDeleted && br.RequesterId == currentUserId);
                
                // Filter by department only if user has a restricted role
                if (shouldFilterByDepartment)
                {
                    requesterQuery = requesterQuery.Where(br => br.DepartmentId == userDepartmentId.Value);
                }
                
                var isRequester = await requesterQuery.AnyAsync();

                // User has permission if they can approve OR they are the requester
                hasPermission = hasWorkflowPermission || isRequester;
            }

            if (!hasPermission)
                return null;

            // Get BaseRequest that the user has permission to view
            var baseRequest = await _context.BaseRequests
                .Include(br => br.Requester)
                .Include(br => br.Department)
                .Include(br => br.RequestPurpose)
                .Where(br => !br.IsDeleted && br.Id == requestId)
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
                    DepartmentNameAr = br.Department != null ? br.Department.NameAr : null,
                    DepartmentNameEn = br.Department != null ? br.Department.NameEn : null,
                    RequesterName = br.Requester != null ? (br.Requester.FullNameEN ?? br.Requester.FullNameAR ?? br.Requester.UserName) : null,
                    RequesterNameEn = br.Requester != null ? br.Requester.FullNameEN : null,
                    RequesterNameAr = br.Requester != null ? br.Requester.FullNameAR : null,
                    RequesterUserName = br.Requester != null ? br.Requester.UserName : null,
                    RequestPurposeName = br.RequestPurpose != null ? br.RequestPurpose.NameEn : null,
                    RequestPurposeNameAr = br.RequestPurpose != null ? br.RequestPurpose.NameAr : null,
                    RequestPurposeNameEn = br.RequestPurpose != null ? br.RequestPurpose.NameEn : null
                })
                .FirstOrDefaultAsync();

            if (baseRequest == null)
                return null;

            // Get approval history for this request
            var approvalHistoryData = await (from log in _context.WorkflowStepApprovalLog
                                            join was in _context.WorkflowApprovalSteps
                                                on log.WorkflowApprovalStepId equals was.Id
                                            join wfs in _context.WorkflowSteps
                                                on log.WorkflowStepId equals wfs.Id into wfsJoin
                                            from wfs in wfsJoin.DefaultIfEmpty()
                                            join role in _context.Roles
                                                on wfs.ApplicationRoleId equals role.Id into roleJoin
                                            from role in roleJoin.DefaultIfEmpty()
                                            join actedRole in _context.Roles
                                                on log.ChangedByRoleId equals actedRole.Id into actedRoleJoin
                                            from actedRole in actedRoleJoin.DefaultIfEmpty()
                                            from user in _context.Users.Where(u => u.Id == log.ChangedBy || u.UserName == log.ChangedBy).DefaultIfEmpty()
                                            where was.TargetRequestId == requestId
                                            select new
                                            {
                                                RequestId = was.TargetRequestId,
                                                WorkflowStepId = log.WorkflowStepId,
                                                History = new ApprovalHistoryDto
                                                {
                                                    Id = log.Id,
                                                    WorkflowApprovalStepId = log.WorkflowApprovalStepId,
                                                    WorkflowStepId = log.WorkflowStepId,
                                                    OldRequestStatus = log.OldRequestStatus,
                                                    NewRequestStatus = log.NewRequestStatus,
                                                    Comments = log.Comments,
                                                    ChangedBy = log.ChangedBy,
                                                    ApproverNameEn = user != null ? user.FullNameEN : null,
                                                    ApproverNameAr = user != null ? user.FullNameAR : null,
                                                    ChangedAt = log.ChangedAt,
                                                    StepOrder = wfs != null ? wfs.StepOrder : null,
                                                    ApplicationRoleId = wfs != null ? wfs.ApplicationRoleId : null,
                                                    ApplicationRoleName = role != null ? role.Name : null,
                                                    ApplicationRoleNameAr = role != null ? role.NameAr : null,
                                                    RequireHigherApproval = wfs != null ? wfs.RequireHigherApproval : false,
                                                    HigherApprovalRoleId = wfs != null ? wfs.HigherApprovalRoleId : null,
                                                    CanReturn = wfs != null ? wfs.CanReturn : false,
                                                    IsDelegation = was.IsDelegation,
                                                    ChangedByRoleId = log.ChangedByRoleId,
                                                    ChangedByRoleName = actedRole != null ? actedRole.Name : null,
                                                    ChangedByRoleNameAr = actedRole != null ? actedRole.NameAr : null,
                                                    Files = new List<FileUploadDto>() // Initialize Files list
                                                }
                                            })
                                            .OrderBy(h => h.History.ChangedAt)
                                            .ToListAsync();

            // Get pending workflow approval steps for this request (before loading transitions)
            var pendingStepsData = await (from was in _context.WorkflowApprovalSteps
                                         join wfs in _context.WorkflowSteps
                                             .Include(ws => ws.ApplicationRole)
                                             .Include(ws => ws.HigherApprovalRole)
                                             on was.WorkflowStepId equals wfs.Id
                                         where was.TargetRequestId == requestId &&
                                               was.IsCurrent &&
                                               (was.Status == RequestStatus.New || was.Status == RequestStatus.UnderProcess)
                                         select new
                                         {
                                             RequestId = was.TargetRequestId,
                                             was.WorkflowStepId,
                                             WorkflowApprovalStepId = was.Id,
                                             was.Status,
                                             was.Comments,
                                             was.CreationDate,
                                             wfs.StepOrder,
                                             was.ApproverUserId,
                                             wfs.ApplicationRoleId,
                                             ApplicationRoleName = wfs.ApplicationRole != null ? wfs.ApplicationRole.Name : null,
                                             ApplicationRoleNameAr = wfs.ApplicationRole != null ? wfs.ApplicationRole.NameAr : null,
                                             wfs.RequireHigherApproval,
                                             wfs.HigherApprovalRoleId,
                                             HigherApprovalRoleName = wfs.HigherApprovalRole != null ? wfs.HigherApprovalRole.Name : null,
                                             HigherApprovalRoleNameAr = wfs.HigherApprovalRole != null ? wfs.HigherApprovalRole.NameAr : null,
                                             wfs.CanReturn
                                         })
                                         .ToListAsync();

            var detailParallelWfStepIds = pendingStepsData.Select(p => p.WorkflowStepId).Distinct().ToList();
            var detailParallelRolesByWfStep = await BuildParallelRolesByStepIdsAsync(detailParallelWfStepIds);
            var detailParallelRoleIdsByWfStep = await BuildParallelRoleIdsByStepIdsAsync(detailParallelWfStepIds);

            // Get all workflow step IDs from approval history and pending steps to load their transitions
            var workflowStepIds = approvalHistoryData
                .Where(h => h.WorkflowStepId.HasValue)
                .Select(h => h.WorkflowStepId.Value)
                .Union(pendingStepsData.Select(p => p.WorkflowStepId))
                .Distinct()
                .ToList();

            // Load transitions for all workflow steps in the approval history
            var workflowStepsWithTransitions = new Dictionary<long, List<WorkflowStepTransitionDto>>();
            if (workflowStepIds.Any())
            {
                var workflowSteps = await _context.WorkflowSteps
                    .Include(ws => ws.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.ApplicationRole)
                    .Include(ws => ws.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.HigherApprovalRole)
                    .Where(ws => workflowStepIds.Contains(ws.Id))
                    .ToListAsync();

                // Create a dictionary for quick lookup
                workflowStepsWithTransitions = workflowSteps
                    .ToDictionary(
                        ws => ws.Id,
                        ws => ws.Transitions.Select(t => new WorkflowStepTransitionDto
                        {
                            Id = t.Id,
                            SourceWorkflowStepId = t.SourceWorkflowStepId,
                            TargetWorkflowStepId = t.TargetWorkflowStepId,
                            TargetStep = t.TargetWorkflowStep != null ? new TargetStepDetailsDto
                            {
                                Id = t.TargetWorkflowStep.Id,
                                WorkflowId = t.TargetWorkflowStep.WorkflowId,
                                StepOrder = t.TargetWorkflowStep.StepOrder,
                                ApplicationRole = t.TargetWorkflowStep.ApplicationRole != null ? new RoleDto
                                {
                                    Id = t.TargetWorkflowStep.ApplicationRole.Id,
                                    Name = t.TargetWorkflowStep.ApplicationRole.Name,
                                    NameAr = t.TargetWorkflowStep.ApplicationRole.NameAr,
                                    IsDefaultRole = t.TargetWorkflowStep.ApplicationRole.IsDefaultRole ?? false,
                                    IsSuperAdmin = t.TargetWorkflowStep.ApplicationRole.IsSuperAdmin,
                                    ApplicationEntityIds = new List<long>()
                                } : null,
                                ApplicationEntityId = t.TargetWorkflowStep.ApplicationEntityId,
                                RequireHigherApproval = t.TargetWorkflowStep.RequireHigherApproval,
                                HigherApprovalRole = t.TargetWorkflowStep.HigherApprovalRole != null ? new RoleDto
                                {
                                    Id = t.TargetWorkflowStep.HigherApprovalRole.Id,
                                    Name = t.TargetWorkflowStep.HigherApprovalRole.Name,
                                    NameAr = t.TargetWorkflowStep.HigherApprovalRole.NameAr,
                                    IsDefaultRole = t.TargetWorkflowStep.HigherApprovalRole.IsDefaultRole ?? false,
                                    IsSuperAdmin = t.TargetWorkflowStep.HigherApprovalRole.IsSuperAdmin,
                                    ApplicationEntityIds = new List<long>()
                                } : null,
                                HigherApplicationEntityId = t.TargetWorkflowStep.HigherApplicationEntityId,
                                MustApprove = t.TargetWorkflowStep.MustApprove,
                                ReserveQty = t.TargetWorkflowStep.ReserveQty,
                                CanSkip = t.TargetWorkflowStep.CanSkip
                            } : null
                        }).ToList()
                    );
            }

            // Get all active workflows with their steps, grouped by WorkflowType
            var workflowsByType = await _context.Workflows
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(ws => ws.ApplicationRole)
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(ws => ws.HigherApprovalRole)
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(ws => ws.ParallelRoles)
                        .ThenInclude(pr => pr.Role)
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(ws => ws.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.ApplicationRole)
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(ws => ws.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.HigherApprovalRole)
                .Where(w => w.IsActive && !w.IsDeleted)
                .GroupBy(w => w.WorkflowType)
                .ToDictionaryAsync(g => g.Key, g => g.FirstOrDefault());

            // Get ALL workflow approval steps for this request
            var allWorkflowApprovalSteps = await _context.WorkflowApprovalSteps
                .Where(was => was.TargetRequestId == requestId)
                .Select(was => new { was.Id, was.TargetRequestId })
                .ToListAsync();

            // Collect all workflow approval step IDs
            var allApprovalStepIds = new HashSet<long>();
            
            foreach (var step in allWorkflowApprovalSteps)
            {
                if (step.Id > 0)
                {
                    allApprovalStepIds.Add(step.Id);
                }
            }
            
            foreach (var historyItem in approvalHistoryData)
            {
                if (historyItem.History.WorkflowApprovalStepId > 0)
                {
                    allApprovalStepIds.Add(historyItem.History.WorkflowApprovalStepId);
                }
            }
            
            foreach (var pendingStep in pendingStepsData)
            {
                if (pendingStep.WorkflowApprovalStepId > 0)
                {
                    allApprovalStepIds.Add(pendingStep.WorkflowApprovalStepId);
                }
            }

            // Get files for all approval steps
            var filesByStepId = new Dictionary<long, List<FileUploadDto>>();
            foreach (var stepId in allApprovalStepIds)
            {
                var filesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.WorkflowApproval, stepId);
                if (filesResult.Succeeded && filesResult.Data != null)
                {
                    filesByStepId[stepId] = filesResult.Data;
                }
                else
                {
                    filesByStepId[stepId] = new List<FileUploadDto>();
                }
            }

            // Get files for order, return, and discard
            if (baseRequest.RequestType == RequestType.Order || 
                baseRequest.RequestType == RequestType.Return || 
                baseRequest.RequestType == RequestType.Discard)
            {
                try
                {
                    var filesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Order, baseRequest.Id);
                    if (filesResult.Succeeded && filesResult.Data != null)
                    {
                        baseRequest.Files = filesResult.Data;
                    }
                    else
                    {
                        baseRequest.Files = new List<FileUploadDto>();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting files for request. RequestId: {RequestId}, RequestType: {RequestType}", 
                        baseRequest.Id, baseRequest.RequestType);
                    baseRequest.Files = new List<FileUploadDto>();
                }
            }
            else
            {
                baseRequest.Files = new List<FileUploadDto>();
            }

            // Assign approval history and merge with all workflow steps
            var combinedHistory = new List<ApprovalHistoryDto>();

            // Get completed approval history and assign files to each step
            foreach (var historyItem in approvalHistoryData)
            {
                // Assign files to this approval step
                if (historyItem.History.WorkflowApprovalStepId > 0 && filesByStepId.TryGetValue(historyItem.History.WorkflowApprovalStepId, out var stepFiles))
                {
                    historyItem.History.Files = stepFiles;
                }
                else
                {
                    historyItem.History.Files = new List<FileUploadDto>();
                }

                // Assign transitions to this approval step if workflow step exists
                if (historyItem.WorkflowStepId.HasValue && workflowStepsWithTransitions.TryGetValue(historyItem.WorkflowStepId.Value, out var transitions))
                {
                    historyItem.History.Transitions = transitions;
                }
                else
                {
                    historyItem.History.Transitions = new List<WorkflowStepTransitionDto>();
                }

                combinedHistory.Add(historyItem.History);
            }

            // Check if request has been rejected
            bool isRejected = baseRequest.Status == RequestStatus.Rejected;
            
            if (!isRejected && combinedHistory.Any(h => h.NewRequestStatus == RequestStatus.Rejected))
            {
                isRejected = true;
            }

            // Only show pending/future steps if request is not rejected
            if (!isRejected)
            {
                // Get the workflow for this request type
                var workflowType = (WorkflowType)baseRequest.RequestType;
                if (workflowsByType.TryGetValue(workflowType, out var workflow) && workflow != null)
                {
                    // Get all workflow steps
                    var allWorkflowSteps = workflow.WorkflowSteps.OrderBy(ws => ws.StepOrder).ToList();

                    // Create a set of workflow step IDs that have been completed or are pending
                    var completedOrPendingStepIds = new HashSet<long>();
                    foreach (var h in combinedHistory)
                    {
                        if (h.WorkflowStepId.HasValue)
                            completedOrPendingStepIds.Add(h.WorkflowStepId.Value);
                    }

                    // Get pending steps for this request and add them
                    var requestPendingSteps = pendingStepsData.Where(p => p.RequestId == requestId).ToList();
                    if (requestPendingSteps.Any())
                    {
                        // Only add the next pending step (the one with the lowest step order that hasn't been completed)
                        var nextPendingStep = requestPendingSteps
                            .OrderBy(p => p.StepOrder)
                            .FirstOrDefault();
                        
                        if (nextPendingStep != null)
                        {
                            completedOrPendingStepIds.Add(nextPendingStep.WorkflowStepId);
                            
                            // Check if this is a higher approval step
                            bool isHigherApprovalStep = nextPendingStep.RequireHigherApproval &&
                                combinedHistory.Any(h => 
                                    h.WorkflowStepId == nextPendingStep.WorkflowStepId && 
                                    h.NewRequestStatus == RequestStatus.Approved);
                            
                            // Use HigherApprovalRoleName if this is a higher approval step, otherwise use ApplicationRoleName
                            string roleNameToUse = isHigherApprovalStep && !string.IsNullOrEmpty(nextPendingStep.HigherApprovalRoleName)
                                ? nextPendingStep.HigherApprovalRoleName
                                : nextPendingStep.ApplicationRoleName;
                            
                            string roleNameArToUse = isHigherApprovalStep && !string.IsNullOrEmpty(nextPendingStep.HigherApprovalRoleNameAr)
                                ? nextPendingStep.HigherApprovalRoleNameAr
                                : nextPendingStep.ApplicationRoleNameAr;
                            
                            // Calculate IsCurrentUserApprover (including delegation)
                            bool isCurrentUserApprover = false;

                            if (_currentUserService.IsSuperAdmin)
                            {
                                isCurrentUserApprover = true;
                            }
                            else if (!string.IsNullOrEmpty(nextPendingStep.ApproverUserId))
                            {
                                // If assigned to specific user, check direct OR delegator
                                isCurrentUserApprover = nextPendingStep.ApproverUserId == currentUserId || 
                                                       activeDelegatorIds.Contains(nextPendingStep.ApproverUserId);
                            }
                            else
                            {
                                // Check role requirements (User Roles OR Delegator Roles)
                                string requiredRoleId = isHigherApprovalStep ? nextPendingStep.HigherApprovalRoleId : nextPendingStep.ApplicationRoleId;
                                string requiredRoleName = roleNameToUse;

                                // Check Role ID match
                                if (!string.IsNullOrEmpty(requiredRoleId))
                                {
                                    if (userRoleIds.Contains(requiredRoleId) || delegatorRoleIds.Contains(requiredRoleId))
                                    {
                                        isCurrentUserApprover = true;
                                    }
                                }
                                
                                // Check Role Name match (if ID didn't match)
                                if (!isCurrentUserApprover && !string.IsNullOrEmpty(requiredRoleName))
                                {
                                     var normalizedRequired = requiredRoleName.ToLower().Replace(" ", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace("(", "").Replace(")", "");
                                     
                                     if (allRelevantRoleNames.Any(r => !string.IsNullOrEmpty(r) && r.ToLower().Replace(" ", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace("(", "").Replace(")", "") == normalizedRequired))
                                     {
                                         isCurrentUserApprover = true;
                                     }
                                }

                                if (!isCurrentUserApprover && !isHigherApprovalStep &&
                                    detailParallelRoleIdsByWfStep.TryGetValue(nextPendingStep.WorkflowStepId, out var detailParallelIds) &&
                                    detailParallelIds.Any(pid =>
                                        userRoleIds.Contains(pid) || delegatorRoleIds.Contains(pid)))
                                {
                                    isCurrentUserApprover = true;
                                }
                            }

                            detailParallelRolesByWfStep.TryGetValue(nextPendingStep.WorkflowStepId, out var detailParallelRolesForPending);
                            detailParallelRolesForPending ??= new List<WorkflowStepParallelRoleDto>();

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
                                ApplicationRoleNameAr = roleNameArToUse,
                                RequireHigherApproval = nextPendingStep.RequireHigherApproval,
                                HigherApprovalRoleId = nextPendingStep.HigherApprovalRoleId,
                                CanReturn = nextPendingStep.CanReturn,
                                IsPending = true,
                                IsCurrentUserApprover = isCurrentUserApprover,
                                EligibleParallelRoles = detailParallelRolesForPending,
                                EligibleParallelRoleNamesEn = string.Join(" | ", detailParallelRolesForPending.Select(x => x.RoleName).Where(x => !string.IsNullOrEmpty(x)).Distinct()),
                                EligibleParallelRoleNamesAr = string.Join(" | ", detailParallelRolesForPending.Select(x => x.RoleNameAr).Where(x => !string.IsNullOrEmpty(x)).Distinct())
                            };
                            
                            // Assign files to pending step if any
                            if (nextPendingStep.WorkflowApprovalStepId > 0 && filesByStepId.TryGetValue(nextPendingStep.WorkflowApprovalStepId, out var pendingStepFiles))
                            {
                                pendingStep.Files = pendingStepFiles;
                            }
                            else
                            {
                                pendingStep.Files = new List<FileUploadDto>();
                            }

                            // Assign transitions to pending step
                            if (nextPendingStep.WorkflowStepId > 0 && workflowStepsWithTransitions.TryGetValue(nextPendingStep.WorkflowStepId, out var pendingTransitions))
                            {
                                pendingStep.Transitions = pendingTransitions;
                            }
                            else
                            {
                                pendingStep.Transitions = new List<WorkflowStepTransitionDto>();
                            }
                            
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
                                ApplicationRoleNameAr = nextWorkflowStep.ApplicationRole != null ? nextWorkflowStep.ApplicationRole.NameAr : null,
                                RequireHigherApproval = nextWorkflowStep.RequireHigherApproval,
                                HigherApprovalRoleId = nextWorkflowStep.HigherApprovalRoleId,
                                CanReturn = nextWorkflowStep.CanReturn,
                                IsPending = true,
                                EligibleParallelRoles = nextWorkflowStep.ParallelRoles != null
                                    ? nextWorkflowStep.ParallelRoles.Select(pr => new WorkflowStepParallelRoleDto
                                    {
                                        Id = pr.Id,
                                        WorkflowStepId = pr.WorkflowStepId,
                                        RoleId = pr.RoleId,
                                        RoleName = pr.Role?.Name ?? pr.RoleId,
                                        RoleNameAr = pr.Role?.NameAr ?? pr.Role?.Name ?? pr.RoleId
                                    }).ToList()
                                    : new List<WorkflowStepParallelRoleDto>(),
                                EligibleParallelRoleNamesEn = nextWorkflowStep.ParallelRoles != null && nextWorkflowStep.ParallelRoles.Count > 0
                                    ? string.Join(" | ", nextWorkflowStep.ParallelRoles.Select(pr => pr.Role?.Name ?? pr.RoleId).Where(x => !string.IsNullOrEmpty(x)).Distinct())
                                    : null,
                                EligibleParallelRoleNamesAr = nextWorkflowStep.ParallelRoles != null && nextWorkflowStep.ParallelRoles.Count > 0
                                    ? string.Join(" | ", nextWorkflowStep.ParallelRoles.Select(pr => pr.Role?.NameAr ?? pr.Role?.Name ?? pr.RoleId).Where(x => !string.IsNullOrEmpty(x)).Distinct())
                                    : null,
                                Files = new List<FileUploadDto>(), // No files for future steps that haven't been created yet
                                Transitions = nextWorkflowStep.Transitions?.Select(t => new WorkflowStepTransitionDto
                                {
                                    Id = t.Id,
                                    SourceWorkflowStepId = t.SourceWorkflowStepId,
                                    TargetWorkflowStepId = t.TargetWorkflowStepId,
                                    TargetStep = t.TargetWorkflowStep != null ? new TargetStepDetailsDto
                                    {
                                        Id = t.TargetWorkflowStep.Id,
                                        WorkflowId = t.TargetWorkflowStep.WorkflowId,
                                        StepOrder = t.TargetWorkflowStep.StepOrder,
                                        ApplicationRole = t.TargetWorkflowStep.ApplicationRole != null ? new RoleDto
                                        {
                                            Id = t.TargetWorkflowStep.ApplicationRole.Id,
                                            Name = t.TargetWorkflowStep.ApplicationRole.Name,
                                            NameAr = t.TargetWorkflowStep.ApplicationRole.NameAr,
                                            IsDefaultRole = t.TargetWorkflowStep.ApplicationRole.IsDefaultRole ?? false,
                                            IsSuperAdmin = t.TargetWorkflowStep.ApplicationRole.IsSuperAdmin,
                                            ApplicationEntityIds = new List<long>()
                                        } : null,
                                        ApplicationEntityId = t.TargetWorkflowStep.ApplicationEntityId,
                                        RequireHigherApproval = t.TargetWorkflowStep.RequireHigherApproval,
                                        HigherApprovalRole = t.TargetWorkflowStep.HigherApprovalRole != null ? new RoleDto
                                        {
                                            Id = t.TargetWorkflowStep.HigherApprovalRole.Id,
                                            Name = t.TargetWorkflowStep.HigherApprovalRole.Name,
                                            NameAr = t.TargetWorkflowStep.HigherApprovalRole.NameAr,
                                            IsDefaultRole = t.TargetWorkflowStep.HigherApprovalRole.IsDefaultRole ?? false,
                                            IsSuperAdmin = t.TargetWorkflowStep.HigherApprovalRole.IsSuperAdmin,
                                            ApplicationEntityIds = new List<long>()
                                        } : null,
                                        HigherApplicationEntityId = t.TargetWorkflowStep.HigherApplicationEntityId,
                                        MustApprove = t.TargetWorkflowStep.MustApprove,
                                        ReserveQty = t.TargetWorkflowStep.ReserveQty,
                                        CanSkip = t.TargetWorkflowStep.CanSkip
                                    } : null
                                }).ToList() ?? new List<WorkflowStepTransitionDto>()
                            };
                            combinedHistory.Add(futureStep);
                        }
                    }
                }
            }

            // Sort: completed steps first (by step order and date), then pending steps last
            baseRequest.ApprovalHistory = combinedHistory
                .OrderBy(h => h.IsPending ? 1 : 0) // Pending steps (1) come after completed steps (0)
                .ThenBy(h => h.StepOrder ?? int.MaxValue)
                .ThenBy(h => h.ChangedAt == DateTime.MinValue ? DateTime.MaxValue : h.ChangedAt)                    
                .ToList();

            if (baseRequest.RequestType == RequestType.Order)
            {
                baseRequest.SupplyDate = await _context.Set<Order>().AsNoTracking()
                    .Where(o => o.Id == requestId)
                    .Select(o => o.SupplyDate)
                    .FirstOrDefaultAsync();
            }

            return baseRequest;
        }

        private async Task EnrichOrderSupplyDatesAsync(List<BaseRequestDto> baseRequests)
        {
            var orderIds = baseRequests.Where(r => r.RequestType == RequestType.Order).Select(r => r.Id).ToList();
            if (orderIds.Count == 0)
                return;

            var map = await _context.Set<Order>().AsNoTracking()
                .Where(o => orderIds.Contains(o.Id))
                .Select(o => new { o.Id, o.SupplyDate })
                .ToDictionaryAsync(x => x.Id, x => x.SupplyDate);

            foreach (var br in baseRequests)
            {
                if (map.TryGetValue(br.Id, out var sd))
                    br.SupplyDate = sd;
            }
        }

        /// <summary>
        /// Get all previous workflow steps that can be returned to for review
        /// </summary>
        public async Task<IEnumerable<WorkflowStepDto>> GetPreviousWorkflowStepsForReturn(long requestId)
        {
            // Get the current approval step for this request
            var currentApprovalStep = await _context.WorkflowApprovalSteps
                .Include(x => x.WorkflowStep)
                .FirstOrDefaultAsync(x => x.TargetRequestId == requestId && x.IsCurrent);

            if (currentApprovalStep == null)
            {
                return Enumerable.Empty<WorkflowStepDto>();
            }

            // Get all workflow steps in the same workflow
            var allWorkflowSteps = await _context.WorkflowSteps
                .Include(ws => ws.ApplicationRole)
                .Include(ws => ws.HigherApprovalRole)
                .Include(ws => ws.Transitions)
                    .ThenInclude(t => t.TargetWorkflowStep)
                        .ThenInclude(target => target.ApplicationRole)
                .Include(ws => ws.Transitions)
                    .ThenInclude(t => t.TargetWorkflowStep)
                        .ThenInclude(target => target.HigherApprovalRole)
                .Where(ws => ws.WorkflowId == currentApprovalStep.WorkflowStep.WorkflowId)
                .OrderBy(ws => ws.StepOrder)
                .ToListAsync();

            // Get only the previous steps (steps with lower StepOrder than current)
            var previousSteps = allWorkflowSteps
                .Where(ws => ws.StepOrder < currentApprovalStep.WorkflowStep.StepOrder)
                .Select(ws => new WorkflowStepDto
                {
                    Id = ws.Id,
                    WorkflowId = ws.WorkflowId,
                    StepOrder = ws.StepOrder,
                    ApplicationRoleId = ws.ApplicationRoleId,
                    ApplicationRoleName = ws.ApplicationRole?.Name,
                    ApplicationRoleNameAr = ws.ApplicationRole?.NameAr,
                    ApplicationEntityId = ws.ApplicationEntityId,
                    MustApprove = ws.MustApprove,
                    RequireHigherApproval = ws.RequireHigherApproval,
                    HigherApprovalRoleId = ws.HigherApprovalRoleId,
                    HigherApplicationEntityId = ws.HigherApplicationEntityId,
                    ReserveQty = ws.ReserveQty,
                    CanSkip = ws.CanSkip,
                    Transitions = ws.Transitions?.Select(t => new WorkflowStepTransitionDto
                    {
                        Id = t.Id,
                        SourceWorkflowStepId = t.SourceWorkflowStepId,
                        TargetWorkflowStepId = t.TargetWorkflowStepId,
                        TargetStep = t.TargetWorkflowStep != null ? new TargetStepDetailsDto
                        {
                            Id = t.TargetWorkflowStep.Id,
                            WorkflowId = t.TargetWorkflowStep.WorkflowId,
                            StepOrder = t.TargetWorkflowStep.StepOrder,
                            ApplicationRole = t.TargetWorkflowStep.ApplicationRole != null ? new RoleDto
                            {
                                Id = t.TargetWorkflowStep.ApplicationRole.Id,
                                Name = t.TargetWorkflowStep.ApplicationRole.Name,
                                NameAr = t.TargetWorkflowStep.ApplicationRole.NameAr,
                                IsDefaultRole = t.TargetWorkflowStep.ApplicationRole.IsDefaultRole ?? false,
                                IsSuperAdmin = t.TargetWorkflowStep.ApplicationRole.IsSuperAdmin,
                                ApplicationEntityIds = new List<long>()
                            } : null,
                            ApplicationEntityId = t.TargetWorkflowStep.ApplicationEntityId,
                            RequireHigherApproval = t.TargetWorkflowStep.RequireHigherApproval,
                            HigherApprovalRole = t.TargetWorkflowStep.HigherApprovalRole != null ? new RoleDto
                            {
                                Id = t.TargetWorkflowStep.HigherApprovalRole.Id,
                                Name = t.TargetWorkflowStep.HigherApprovalRole.Name,
                                NameAr = t.TargetWorkflowStep.HigherApprovalRole.NameAr,
                                IsDefaultRole = t.TargetWorkflowStep.HigherApprovalRole.IsDefaultRole ?? false,
                                IsSuperAdmin = t.TargetWorkflowStep.HigherApprovalRole.IsSuperAdmin,
                                ApplicationEntityIds = new List<long>()
                            } : null,
                            HigherApplicationEntityId = t.TargetWorkflowStep.HigherApplicationEntityId,
                            MustApprove = t.TargetWorkflowStep.MustApprove,
                            ReserveQty = t.TargetWorkflowStep.ReserveQty,
                            CanSkip = t.TargetWorkflowStep.CanSkip
                        } : null
                    }).ToList() ?? new List<WorkflowStepTransitionDto>()
                })
                .ToList();

            return previousSteps;
        }

        private static List<string> CollectApproverRoleIdsForWorkflowStep(WorkflowStep wfs)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(wfs.ApplicationRoleId))
                list.Add(wfs.ApplicationRoleId);
            if (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId))
                list.Add(wfs.HigherApprovalRoleId);
            if (wfs.ParallelRoles != null)
            {
                foreach (var pr in wfs.ParallelRoles)
                {
                    if (!string.IsNullOrEmpty(pr.RoleId))
                        list.Add(pr.RoleId);
                }
            }
            return list.Distinct().ToList();
        }

        private async Task<bool> TryAuthorizeWorkflowApprovalStepAsync(WorkflowApprovalStep step, string currentUserId)
        {
            var userRoleIds = string.IsNullOrEmpty(currentUserId)
                ? new List<string>()
                : (await _effectiveRoleService.GetEffectiveRoleIdsAsync(currentUserId)).ToList();

            var workflowStep = step.WorkflowStep;
            if (workflowStep == null)
                return false;

            var allowedRoles = CollectApproverRoleIdsForWorkflowStep(workflowStep)
                .Where(id => !string.IsNullOrEmpty(id))
                .Distinct()
                .ToList();

            if ((step.ApproverUserId == currentUserId) || userRoleIds.Any(r => allowedRoles.Contains(r)))
            {
                step.IsDelegation = false;
                return true;
            }

            var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(currentUserId, DelegationScope.WorkflowApproval);
            if (activeDelegatorIds == null || !activeDelegatorIds.Any())
                return false;

            if (activeDelegatorIds.Contains(step.ApproverUserId))
            {
                step.IsDelegation = true;
                return true;
            }

            var delegatorRoleIds = new List<string>();
            foreach (var delegatorId in activeDelegatorIds)
                delegatorRoleIds.AddRange(await _effectiveRoleService.GetEffectiveRoleIdsAsync(delegatorId));

            if (delegatorRoleIds.Any(r => allowedRoles.Contains(r)))
            {
                step.IsDelegation = true;
                return true;
            }

            return false;
        }

        private async Task<string?> ResolveActingRoleIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return null;
            if (!string.IsNullOrEmpty(user.DefaultRoleId))
                return user.DefaultRoleId;
            var roles = await _context.Set<IdentityUserRole<string>>()
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();
            return roles.Count == 1 ? roles[0] : null;
        }

        private async Task<Dictionary<long, List<WorkflowStepParallelRoleDto>>> BuildParallelRolesByStepIdsAsync(List<long> stepIds)
        {
            if (stepIds == null || stepIds.Count == 0)
                return new Dictionary<long, List<WorkflowStepParallelRoleDto>>();
            var distinctIds = stepIds.Distinct().ToList();
            var rows = await _context.WorkflowStepParallelRoles
                .Where(pr => distinctIds.Contains(pr.WorkflowStepId))
                .Join(_context.Roles, pr => pr.RoleId, r => r.Id, (pr, r) => new WorkflowStepParallelRoleDto
                {
                    Id = pr.Id,
                    WorkflowStepId = pr.WorkflowStepId,
                    RoleId = pr.RoleId,
                    RoleName = r.Name ?? r.Id,
                    RoleNameAr = r.NameAr ?? r.Name
                })
                .ToListAsync();
            return rows
                .GroupBy(x => x.WorkflowStepId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        private async Task<Dictionary<long, List<string>>> BuildParallelRoleIdsByStepIdsAsync(List<long> stepIds)
        {
            if (stepIds == null || stepIds.Count == 0)
                return new Dictionary<long, List<string>>();
            var distinctIds = stepIds.Distinct().ToList();
            return await _context.WorkflowStepParallelRoles
                .Where(pr => distinctIds.Contains(pr.WorkflowStepId))
                .GroupBy(pr => pr.WorkflowStepId)
                .ToDictionaryAsync(g => g.Key, g => g.Select(x => x.RoleId).ToList());
        }
    }
}
