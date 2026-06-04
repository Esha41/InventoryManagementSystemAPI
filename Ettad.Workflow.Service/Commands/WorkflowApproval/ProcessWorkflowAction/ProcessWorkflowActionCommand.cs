using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Utilities;
using Ettad.Data.Constants;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Data.Interfaces.Services;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Notification.Service.Interfaces;
using Ettad.Workflow.Service.Interface;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Dtos;
using Ettad.Workflows.Service.Events;
using Ettad.Workflows.Service.Queries.WorkflowApproval.GetCurrentApprovalStepByRequestId;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Commands.WorkflowApproval.ProcessWorkflowAction
{
    public class ProcessWorkflowActionCommand : IRequest<APIOperationResponse<bool>>
    {
        public ApproveRejectWorkflowApprovalDto Dto { get; }
        public List<IFormFile>? Files { get; }

        public ProcessWorkflowActionCommand(ApproveRejectWorkflowApprovalDto dto, List<IFormFile>? files = null)
        {
            Dto = dto;
            Files = files;
        }
    }

    public class ProcessWorkflowActionCommandHandler : IRequestHandler<ProcessWorkflowActionCommand, APIOperationResponse<bool>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICrossCuttingRepository<FileUplodDetails> _fileDetailsRepository;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly IWorkflowStepNotifierService _workflowStepNotifierService;
        private readonly IOrderItemTrackingService _orderItemTrackingService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITransactionManager _transactionManager;
        private readonly IMediator _mediator;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<ProcessWorkflowActionCommandHandler> _logger;

        public ProcessWorkflowActionCommandHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IFileUploadService fileUploadService,
            ICrossCuttingRepository<FileUplodDetails> fileDetailsRepository,
            INotificationHelperService notificationHelperService,
            IWorkflowStepNotifierService workflowStepNotifierService,
            IOrderItemTrackingService orderItemTrackingService,
            IDateTimeProvider dateTimeProvider,
            ITransactionManager transactionManager,
            IMediator mediator,
            IPermissionService permissionService,
            ILogger<ProcessWorkflowActionCommandHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _fileUploadService = fileUploadService;
            _fileDetailsRepository = fileDetailsRepository;
            _notificationHelperService = notificationHelperService;
            _workflowStepNotifierService = workflowStepNotifierService;
            _orderItemTrackingService = orderItemTrackingService;
            _dateTimeProvider = dateTimeProvider;
            _transactionManager = transactionManager;
            _mediator = mediator;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<bool>> Handle(ProcessWorkflowActionCommand request, CancellationToken cancellationToken)
        {
            var model = request.Dto;
            var files = request.Files;

            var ownsTransaction = !_transactionManager.HasActiveTransaction;
            if (ownsTransaction)
            {
                await _transactionManager.BeginAsync(cancellationToken);
            }

            try
            {
                List<long>? savedFileMasterIds = null;

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

                var actingUserId = _currentUserService.UserId;
                var actingRoleId = await ResolveActingRoleIdAsync(actingUserId);

                WorkflowApprovalStep currentStep;

                if (model.Action == RequestStatus.Cancelled)
                {
                    if (!_currentUserService.IsSuperAdmin &&
                        !await _permissionService.HasPermissionAsync(PlainPermissions.CanCancelRequest.ToString()))
                    {
                        return APIOperationResponse<bool>.Fail(ResponseType.Forbidden,
                            "You do not have permission to cancel this request.");
                    }

                    if (files == null || files.Count == 0)
                    {
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                            "At least one attachment is required to cancel this request.");
                    }

                    currentStep = await CancelWorkflowRequestCoreAsync(model, actingUserId, actingRoleId, cancellationToken);
                }
                else
                {
                    // Optimistic concurrency: if the client told us which step it was looking at and that
                    // step is no longer current (another approver already actioned it and the workflow moved
                    // on), reject cleanly with 409 instead of acting on whatever step happens to be current now.
                    if (model.ExpectedWorkflowApprovalStepId.HasValue)
                    {
                        var expectedStepId = model.ExpectedWorkflowApprovalStepId.Value;
                        var expectedStepStillCurrent = await _context.WorkflowApprovalSteps
                            .AsNoTracking()
                            .AnyAsync(x => x.Id == expectedStepId &&
                                           x.TargetRequestId == model.BaseRequestID &&
                                           x.IsCurrent &&
                                           (x.Status == RequestStatus.New || x.Status == RequestStatus.UnderProcess),
                                cancellationToken);

                        if (!expectedStepStillCurrent)
                        {
                            if (ownsTransaction)
                                await _transactionManager.RollbackAsync(cancellationToken);
                            return APIOperationResponse<bool>.Fail(ResponseType.Conflict,
                                "This request has already been actioned by another approver and has moved to the next step. The page has been refreshed with the latest status.");
                        }
                    }

                    currentStep = await _mediator.Send(
                        new GetCurrentApprovalStepByRequestIdQuery(model.BaseRequestID),
                        cancellationToken);
                    if (currentStep == null)
                        return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "No current workflow step found for this request.");

                    var oldStatus = currentStep.Status;

                    currentStep.Comments = model.Comments;

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

                    await LogStepActionAsync(currentStep.Id, currentStep.WorkflowStepId, oldStatus, model.Action,
                        model.Comments, actingUserId, actingRoleId);
                }

                await _context.SaveChangesAsync(cancellationToken);
                if (ownsTransaction)
                {
                    await _transactionManager.CommitAsync(cancellationToken);
                }

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
                                EntityId = currentStep.Id
                            };

                            await _fileDetailsRepository.AddAsync(detail);
                        }
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "Exception occurred while linking files to workflow approval step. StepId: {StepId}, RequestId: {RequestId}, Action: {Action}",
                            currentStep.Id,
                            model.BaseRequestID,
                            model.Action);
                    }
                }

                await SendNotificationsToStepNotifiersAsync(currentStep.WorkflowStepId, model);

                // Live update: tell every client currently viewing this request that its workflow
                // state changed, so their open page re-fetches instead of acting on stale data.
                // Best-effort only — a hub failure must never affect the already-committed action.
                try
                {
                    await _notificationHelperService.SendWorkflowStateChangedAsync(model.BaseRequestID);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "Failed to broadcast workflow state change. RequestId: {RequestId}", model.BaseRequestID);
                }

                return APIOperationResponse<bool>.Success(true);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (ownsTransaction)
                    await _transactionManager.RollbackAsync(cancellationToken);
                // The caller is authenticated but not allowed to action this step (e.g. the step
                // advanced to another role). This is an authorization failure (403), NOT an
                // authentication failure (401) — returning 401 would make the SPA treat it as an
                // expired token and force the user to log out.
                return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                if (ownsTransaction)
                    await _transactionManager.RollbackAsync(cancellationToken);
                return APIOperationResponse<bool>.Fail(ResponseType.NotFound, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                if (ownsTransaction)
                    await _transactionManager.RollbackAsync(cancellationToken);
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                if (ownsTransaction)
                    await _transactionManager.RollbackAsync(cancellationToken);
                _logger.LogError(ex,
                    "An error occurred while processing workflow action. RequestId: {RequestId}, Action: {Action}",
                    model.BaseRequestID, model.Action);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"Processing failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancels all current pending workflow steps, updates base request, releases order draft fulfillment, logs history per step, notifies requester.
        /// </summary>
        private async Task<WorkflowApprovalStep> CancelWorkflowRequestCoreAsync(
            ApproveRejectWorkflowApprovalDto model,
            string? actingUserId,
            string? actingRoleId,
            CancellationToken cancellationToken)
        {
            var baseRequest = await _context.BaseRequests
                .FirstOrDefaultAsync(x => x.Id == model.BaseRequestID && !x.IsDeleted, cancellationToken);
            if (baseRequest == null)
                throw new KeyNotFoundException($"Request with ID {model.BaseRequestID} not found.");

            if (baseRequest.Status is RequestStatus.Approved or RequestStatus.Rejected or RequestStatus.Cancelled
                or RequestStatus.AutoRejected)
            {
                throw new InvalidOperationException(
                    "This request cannot be cancelled because it has already been completed or terminated.");
            }

            var pendingSteps = await _context.WorkflowApprovalSteps
                .Include(x => x.WorkflowStep!)
                    .ThenInclude(ws => ws.ApplicationRole)
                .Include(x => x.WorkflowStep!)
                    .ThenInclude(ws => ws.ParallelRoles)
                .Include(x => x.WorkflowStep!)
                    .ThenInclude(ws => ws.Transitions)
                .Where(x => x.TargetRequestId == model.BaseRequestID &&
                            x.IsCurrent &&
                            x.Status != RequestStatus.Approved &&
                            x.Status != RequestStatus.Rejected &&
                            x.Status != RequestStatus.Cancelled &&
                            x.Status != RequestStatus.AutoRejected)
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            if (!pendingSteps.Any())
                throw new InvalidOperationException("No active workflow step found for this request.");

            foreach (var step in pendingSteps)
            {
                var oldStatus = step.Status;
                step.Status = RequestStatus.Cancelled;
                step.ApproverUserId = actingUserId;
                step.ApproverRoleId = actingRoleId;
                step.ApprovedDate = _dateTimeProvider.Now;
                step.IsCurrent = false;
                step.Comments = model.Comments ?? step.Comments;
                step.ModifiedBy = actingUserId;
                step.ModificationDate = _dateTimeProvider.Now;

                await LogStepActionAsync(step.Id, step.WorkflowStepId, oldStatus, RequestStatus.Cancelled,
                    model.Comments, actingUserId, actingRoleId);
            }

            baseRequest.Status = RequestStatus.Cancelled;
            baseRequest.ModifiedBy = actingUserId;
            baseRequest.ModificationDate = _dateTimeProvider.Now;

            if (baseRequest.RequestType == RequestType.Order)
                await ReleaseOrderDraftFulfillmentAsync(baseRequest.Id, cancellationToken);

            var cancelMsg = $"Request #{baseRequest.RequestNo} has been cancelled" +
                (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}");

            await _notificationHelperService.SendNotificationAsync(
                $"Request #{baseRequest.RequestNo} Cancelled",
                cancelMsg,
                "Request",
                baseRequest.Id,
                new List<string> { baseRequest.CreatedBy },
                null,
                actingUserId
            );

            return pendingSteps[0];
        }

        /// <summary>
        /// Removes draft supply (lot reservations), weapon batch selections, and draft asset supplies for an order.
        /// </summary>
        private async Task ReleaseOrderDraftFulfillmentAsync(long orderId, CancellationToken cancellationToken)
        {
            var draftSupply = await _context.Supplies
                .Include(s => s.SupplyDetails)
                .FirstOrDefaultAsync(
                    s => s.OrderId == orderId && s.SubmissionStatus == SupplySubmissionStatus.Draft,
                    cancellationToken);

            if (draftSupply != null)
            {
                if (draftSupply.SupplyDetails != null && draftSupply.SupplyDetails.Any())
                    _context.SupplyDetails.RemoveRange(draftSupply.SupplyDetails);
                _context.Supplies.Remove(draftSupply);
            }

            var weaponSelections = await _context.WeaponSupplySelections
                .Where(ws => ws.OrderId == orderId)
                .ToListAsync(cancellationToken);
            if (weaponSelections.Count > 0)
                _context.WeaponSupplySelections.RemoveRange(weaponSelections);

            var draftAssetSupplies = await _context.AssetSupplies
                .Include(a => a.SupplyDetails)
                .Where(a => a.OrderId == orderId && a.SubmissionStatus == SupplySubmissionStatus.Draft)
                .ToListAsync(cancellationToken);

            foreach (var assetSupply in draftAssetSupplies)
            {
                if (assetSupply.SupplyDetails != null && assetSupply.SupplyDetails.Any())
                    _context.AssetSupplyDetails.RemoveRange(assetSupply.SupplyDetails);
                _context.AssetSupplies.Remove(assetSupply);
            }
        }

        private async Task ApproveStepAsync(WorkflowApprovalStep step, ApproveRejectWorkflowApprovalDto model, string? approverRoleId)
        {
            if (step.Status == RequestStatus.Approved || step.Status == RequestStatus.Rejected)
            {
                throw new InvalidOperationException("This step has already been processed.");
            }

            if (!step.IsCurrent)
            {
                throw new InvalidOperationException("This step is not active anymore.");
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

            if (model.SendToHigherApproval == true)
            {
                bool created = await HandleHigherApprovalAsync(step, baseRequest);

                if (created)
                {
                    await TryNotifyRequesterAfterQtyConfiguredStepApprovalAsync(step.WorkflowStepId, baseRequest);
                    return;
                }
            }

            var workflowSteps = await _context.WorkflowSteps
                .Include(ws => ws.ParallelRoles)
                .Where(ws => ws.WorkflowId == step.WorkflowStep.WorkflowId)
                .OrderBy(ws => ws.StepOrder)
                .ToListAsync();

            WorkflowStep? nextStep = null;
            long? nextReturnToStepId = null;

            if (step.ReturnToStepId.HasValue)
            {
                var targetStep = workflowSteps.FirstOrDefault(ws => ws.Id == step.ReturnToStepId.Value);
                var sequentialNextStep = workflowSteps.FirstOrDefault(ws => ws.StepOrder > step.WorkflowStep.StepOrder);

                if (sequentialNextStep != null && targetStep != null)
                {
                    if (sequentialNextStep.StepOrder <= targetStep.StepOrder)
                    {
                        nextStep = sequentialNextStep;

                        if (sequentialNextStep.Id != step.ReturnToStepId.Value)
                        {
                            nextReturnToStepId = step.ReturnToStepId.Value;
                        }
                    }
                    else
                    {
                        nextStep = sequentialNextStep;
                    }
                }
            }
            else
            {
                if (step.WorkflowStep.CanSkip)
                {
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
                        if (step.WorkflowStep.Transitions.Count == 1)
                        {
                            var targetId = step.WorkflowStep.Transitions.First().TargetWorkflowStepId;
                            nextStep = workflowSteps.FirstOrDefault(ws => ws.Id == targetId);
                        }
                        else
                        {
                            nextStep = workflowSteps.FirstOrDefault(ws => ws.StepOrder > step.WorkflowStep.StepOrder);
                        }
                    }
                }
                else
                {
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
                    ReturnToStepId = nextReturnToStepId,
                    CreatedBy = _currentUserService.UserId,
                    CreationDate = _dateTimeProvider.Now
                };

                _context.WorkflowApprovalSteps.Add(nextApproval);
                baseRequest.Status = RequestStatus.UnderProcess;

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

                await SendNotificationsToStepNotifiersOnWorkflowStartAsync(nextStep.Id, baseRequest.Id);
            }
            else
            {
                baseRequest.Status = RequestStatus.Approved;

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

                var approvedMsg = $"Request #{baseRequest.RequestNo} has been approved" +
                    (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}");

                await _notificationHelperService.SendNotificationAsync(
                    $"Request #{baseRequest.RequestNo} Approved",
                    approvedMsg,
                    "Request",
                    baseRequest.Id,
                    new List<string> { baseRequest.CreatedBy },
                    null,
                    _currentUserService.UserId
                );
            }

            baseRequest.ModifiedBy = _currentUserService.UserId;
            baseRequest.ModificationDate = _dateTimeProvider.Now;

            await TryNotifyRequesterAfterQtyConfiguredStepApprovalAsync(step.WorkflowStepId, baseRequest);

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

        private async Task RejectStepAsync(WorkflowApprovalStep step, ApproveRejectWorkflowApprovalDto model, string? approverRoleId)
        {
            if (step.Status == RequestStatus.Approved || step.Status == RequestStatus.Rejected)
            {
                throw new InvalidOperationException("This step has already been processed.");
            }

            if (!step.IsCurrent)
            {
                throw new InvalidOperationException("This step is not active anymore.");
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

            var rejectedMsg = $"Request #{baseRequest.RequestNo} has been rejected" +
                (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}");

            await _notificationHelperService.SendNotificationAsync(
                $"Request #{baseRequest.RequestNo} Rejected",
                rejectedMsg,
                "Request",
                baseRequest.Id,
                new List<string> { baseRequest.CreatedBy },
                null,
                _currentUserService.UserId
            );

            baseRequest.ModifiedBy = _currentUserService.UserId;
            baseRequest.ModificationDate = _dateTimeProvider.Now;

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

        private async Task ReturnStepAsync(WorkflowApprovalStep step, ApproveRejectWorkflowApprovalDto model, string? approverRoleId)
        {
            if (step.Status == RequestStatus.Approved || step.Status == RequestStatus.Rejected)
            {
                throw new InvalidOperationException("This step has already been processed.");
            }

            if (!step.IsCurrent)
            {
                throw new InvalidOperationException("This step is not active anymore.");
            }

            if (!step.WorkflowStep.CanReturn)
            {
                throw new InvalidOperationException("This workflow step does not allow returning. CanReturn is set to false.");
            }

            if (!model.ReturnToWorkflowStepId.HasValue)
            {
                throw new InvalidOperationException("ReturnToWorkflowStepId must be specified for return action.");
            }

            var returnToWorkflowStep = await _context.WorkflowSteps
                .Include(ws => ws.ParallelRoles)
                .FirstOrDefaultAsync(ws => ws.Id == model.ReturnToWorkflowStepId.Value);

            if (returnToWorkflowStep == null)
            {
                throw new KeyNotFoundException($"Workflow step with ID {model.ReturnToWorkflowStepId.Value} not found.");
            }

            if (returnToWorkflowStep.WorkflowId != step.WorkflowStep.WorkflowId)
            {
                throw new InvalidOperationException("Cannot return to a step in a different workflow.");
            }

            if (returnToWorkflowStep.StepOrder >= step.WorkflowStep.StepOrder)
            {
                throw new InvalidOperationException("Can only return to a previous step.");
            }

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

            var returnApproval = new WorkflowApprovalStep
            {
                WorkflowStepId = returnToWorkflowStep.Id,
                TargetRequestId = step.TargetRequestId,
                RequestType = step.RequestType,
                Status = RequestStatus.New,
                IsCurrent = true,
                ReturnToStepId = step.WorkflowStepId,
                CreatedBy = _currentUserService.UserId,
                CreationDate = _dateTimeProvider.Now
            };

            _context.WorkflowApprovalSteps.Add(returnApproval);
            baseRequest.Status = RequestStatus.ReturnedForReview;

            var returnRoles = CollectApproverRoleIdsForWorkflowStep(returnToWorkflowStep);

            var (userIds, roleIds) = await FilterNotificationRecipientsByDepartmentAsync(returnRoles, baseRequest.DepartmentId);

            var returnedMsg = $"Request #{baseRequest.RequestNo} has been returned for review" +
                (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}");

            await _notificationHelperService.SendNotificationAsync(
                $"Request #{baseRequest.RequestNo} Returned for Review",
                returnedMsg,
                "Request",
                baseRequest.Id,
                userIds,
                roleIds,
                _currentUserService.UserId
            );

            await SendNotificationsToStepNotifiersOnWorkflowStartAsync(returnToWorkflowStep.Id, baseRequest.Id);

            baseRequest.ModifiedBy = _currentUserService.UserId;
            baseRequest.ModificationDate = _dateTimeProvider.Now;
        }

        private async Task TryNotifyRequesterAfterQtyConfiguredStepApprovalAsync(long workflowStepId, BaseRequest baseRequest)
        {
            try
            {
                var configured = await _context.WorkflowStepRequesterQuantityNotifications
                    .AsNoTracking()
                    .AnyAsync(x => x.WorkflowStepId == workflowStepId);

                if (!configured || string.IsNullOrEmpty(baseRequest.RequesterId))
                    return;

                var title = $"Order #{baseRequest.RequestNo} QTY Updated";
                string message;
                if (baseRequest.RequestType == RequestType.Order)
                {
                    var lines = await _context.RequestItems
                        .AsNoTracking()
                        .Include(ri => ri.Item)
                        .Where(ri => ri.RequestId == baseRequest.Id && !ri.IsDeleted)
                        .OrderBy(ri => ri.Id)
                        .ToListAsync();

                    var itemIds = lines.Select(ri => ri.Id).ToList();
                    var histories = itemIds.Count == 0
                        ? new List<OrderItemHistory>()
                        : await _context.OrderItemHistory
                            .AsNoTracking()
                            .Where(h =>
                                h.OrderId == baseRequest.Id &&
                                h.RequestItemId.HasValue &&
                                itemIds.Contains(h.RequestItemId.Value) &&
                                (h.ActionType == OrderItemActionType.Added ||
                                 h.ActionType == OrderItemActionType.QuantityModified))
                            .ToListAsync();

                    var byRequestItemId = histories
                        .GroupBy(h => h.RequestItemId!.Value)
                        .ToDictionary(
                            g => g.Key,
                            g => g.OrderBy(h => h.ActionDate).ThenBy(h => h.Id).ToList());

                    var parts = lines.Select(ri =>
                    {
                        var name = ri.Item?.Name ?? $"Item {ri.ItemId}";
                        var no = ri.Item?.ItemNo;
                        var label = string.IsNullOrWhiteSpace(no) ? name : $"{name} ({no})";
                        byRequestItemId.TryGetValue(ri.Id, out var itemHistories);
                        var requestedQty = ResolveRequestedQuantity(ri.Quantity, itemHistories);
                        var approvedQty = ri.Quantity;
                        return requestedQty != approvedQty
                            ? $"• {label}: requested quantity {requestedQty} → approved quantity {approvedQty}"
                            : $"• {label}: quantity remains {approvedQty}";
                    });

                    message =
                        $"Your order #{baseRequest.RequestNo} has been updated.\n\n" +
                        "The following line quantities were adjusted:\n" +
                        string.Join("\n", parts);
                }
                else
                {
                    message =
                        $"Your request #{baseRequest.RequestNo} has been updated.";
                }

                await _notificationHelperService.SendNotificationAsync(
                    title,
                    message,
                    nameof(Order),
                    baseRequest.Id,
                    new List<string> { baseRequest.RequesterId },
                    null,
                    _currentUserService.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Requester notification for qty-configured workflow step failed. RequestId: {RequestId}, WorkflowStepId: {WorkflowStepId}",
                    baseRequest.Id,
                    workflowStepId);
            }
        }

        /// <summary>
        /// Quantity the requester originally asked for (first Added row), or before the first tracked quantity change if Added is missing.
        /// </summary>
        private static long ResolveRequestedQuantity(long currentQuantity, List<OrderItemHistory>? itemHistories)
        {
            if (itemHistories == null || itemHistories.Count == 0)
                return currentQuantity;

            var added = itemHistories.FirstOrDefault(h => h.ActionType == OrderItemActionType.Added);
            if (added?.NewQuantity is long addedQty)
                return addedQty;

            var firstMod = itemHistories.FirstOrDefault(h => h.ActionType == OrderItemActionType.QuantityModified);
            if (firstMod?.PreviousQuantity is long prevQty)
                return prevQty;

            return currentQuantity;
        }

        private async Task<bool> HandleHigherApprovalAsync(WorkflowApprovalStep step, BaseRequest baseRequest)
        {
            if (!step.WorkflowStep.RequireHigherApproval ||
                string.IsNullOrEmpty(step.WorkflowStep.HigherApprovalRoleId))
                return false;

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

        private async Task<(List<string>? userIds, List<string>? roleIds)> FilterNotificationRecipientsByDepartmentAsync(
            List<string> roleIds,
            long? departmentId)
        {
            if (roleIds == null || !roleIds.Any())
                return (null, null);

            var restrictedRoleNames = new List<string>
            {
                WorkflowRoleNames.SupplyOfficer,
                WorkflowRoleNames.RequestingEntityCommander
            };

            var roles = await _context.Roles
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync();

            var restrictedRoleIds = new List<string>();
            var nonRestrictedRoleIds = new List<string>();
            var userIds = new List<string>();

            foreach (var role in roles)
            {
                if (restrictedRoleNames.Contains(role.Name))
                    restrictedRoleIds.Add(role.Id);
                else
                    nonRestrictedRoleIds.Add(role.Id);
            }

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

            if (userIds.Any())
                return (userIds, nonRestrictedRoleIds.Any() ? nonRestrictedRoleIds : null);

            return (null, roleIds);
        }

        private async Task SendNotificationsToStepNotifiersAsync(long workflowStepId, ApproveRejectWorkflowApprovalDto model)
        {
            try
            {
                var notifiersResult = await _workflowStepNotifierService.GetNotifierIdsByStepIdAsync(workflowStepId);

                if (!notifiersResult.Succeeded ||
                    notifiersResult.Data.UserIds.Count == 0 && notifiersResult.Data.RoleIds.Count == 0)
                {
                    return;
                }

                var (userIds, roleIds) = notifiersResult.Data;

                var baseRequest = await _context.BaseRequests
                    .FirstOrDefaultAsync(x => x.Id == model.BaseRequestID);

                if (baseRequest == null)
                {
                    return;
                }

                var (title, message) = model.Action switch
                {
                    RequestStatus.Approved => (
                        $"Request #{baseRequest.RequestNo} Approved",
                        $"Request #{baseRequest.RequestNo} has been approved" +
                        (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}")
                    ),
                    RequestStatus.Rejected => (
                        $"Request #{baseRequest.RequestNo} Rejected",
                        $"Request #{baseRequest.RequestNo} has been rejected" +
                        (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}")
                    ),
                    RequestStatus.Cancelled => (
                        $"Request #{baseRequest.RequestNo} Cancelled",
                        $"Request #{baseRequest.RequestNo} has been cancelled" +
                        (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}")
                    ),
                    RequestStatus.ReturnedForReview => (
                        $"Request #{baseRequest.RequestNo} Returned for Review",
                        $"Request #{baseRequest.RequestNo} has been returned for review" +
                        (string.IsNullOrWhiteSpace(model.Comments) ? "." : $". Comments: {model.Comments}")
                    ),
                    _ => (
                        $"Request #{baseRequest.RequestNo} Action Taken",
                        $"An action has been taken on request #{baseRequest.RequestNo}."
                    )
                };

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
                _logger.LogError(ex,
                    "Error sending notifications to step notifiers for workflow step {WorkflowStepId}, RequestId: {RequestId}",
                    workflowStepId,
                    model.BaseRequestID);
            }
        }

        private async Task SendNotificationsToStepNotifiersOnWorkflowStartAsync(long workflowStepId, long requestId)
        {
            try
            {
                var notifiersResult = await _workflowStepNotifierService.GetNotifierIdsByStepIdAsync(workflowStepId);

                if (!notifiersResult.Succeeded ||
                    notifiersResult.Data.UserIds.Count == 0 && notifiersResult.Data.RoleIds.Count == 0)
                {
                    return;
                }

                var (userIds, roleIds) = notifiersResult.Data;

                var baseRequest = await _context.BaseRequests
                    .FirstOrDefaultAsync(x => x.Id == requestId);

                if (baseRequest == null)
                {
                    return;
                }

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
                _logger.LogError(ex,
                    "Error sending start notifications to step notifiers for workflow step {WorkflowStepId}, RequestId: {RequestId}",
                    workflowStepId,
                    requestId);
            }
        }
    }
}
