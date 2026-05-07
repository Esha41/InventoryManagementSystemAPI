using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.CrossCutting.Comman.Time;
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

                var currentStep = await _mediator.Send(
                    new GetCurrentApprovalStepByRequestIdQuery(model.BaseRequestID),
                    cancellationToken);
                if (currentStep == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "No current workflow step found for this request.");

                var oldStatus = currentStep.Status;

                currentStep.Comments = model.Comments;

                var actingUserId = _currentUserService.UserId;
                var actingRoleId = await ResolveActingRoleIdAsync(actingUserId);

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

                await LogStepActionAsync(currentStep.Id, currentStep.WorkflowStepId, oldStatus, model.Action, model.Comments, actingUserId, actingRoleId);

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

                return APIOperationResponse<bool>.Success(true);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (ownsTransaction)
                    await _transactionManager.RollbackAsync(cancellationToken);
                return APIOperationResponse<bool>.Fail(ResponseType.Unauthorized, ex.Message);
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

            await SendNotificationsToStepNotifiersOnWorkflowStartAsync(returnToWorkflowStep.Id, baseRequest.Id);

            baseRequest.ModifiedBy = _currentUserService.UserId;
            baseRequest.ModificationDate = _dateTimeProvider.Now;
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

                var approver = await _context.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);
                var approverName = approver?.FullNameEN ?? approver?.FullNameAR ?? approver?.UserName ?? "System";

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
                    _ => (
                        $"Request #{baseRequest.RequestNo} Action Taken",
                        $"An action has been taken on request #{baseRequest.RequestNo} by {approverName}"
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
