using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Constants;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Data.Interfaces.Services;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Ettad.Workflows.Service.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Queries.WorkflowApproval.GetBaseRequestById
{
    public class GetBaseRequestByIdQuery : IRequest<APIOperationResponse<BaseRequestDto>>
    {
        public long RequestId { get; }

        public GetBaseRequestByIdQuery(long requestId)
        {
            RequestId = requestId;
        }
    }

    public class GetBaseRequestByIdQueryHandler : IRequestHandler<GetBaseRequestByIdQuery, APIOperationResponse<BaseRequestDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserDelegationService _userDelegationService;
        private readonly IEffectiveRoleRepository _effectiveRoleService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<GetBaseRequestByIdQueryHandler> _logger;

        public GetBaseRequestByIdQueryHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IUserDelegationService userDelegationService,
            IEffectiveRoleRepository effectiveRoleService,
            IFileUploadService fileUploadService,
            ILogger<GetBaseRequestByIdQueryHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userDelegationService = userDelegationService;
            _effectiveRoleService = effectiveRoleService;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<BaseRequestDto>> Handle(GetBaseRequestByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var requestId = request.RequestId;
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
                        .ToListAsync(cancellationToken));
                }
                foreach (var delegatorId in activeDelegatorIds)
                {
                    var dRoleIds = await _effectiveRoleService.GetEffectiveRoleIdsAsync(delegatorId);
                    if (dRoleIds.Count > 0)
                    {
                        allRelevantRoleNames.AddRange(await _context.Roles
                            .Where(r => dRoleIds.Contains(r.Id))
                            .Select(r => r.Name!)
                            .ToListAsync(cancellationToken));
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
                        .ToListAsync(cancellationToken);

                var shouldFilterByDepartment = userDepartmentId.HasValue && 
                    userRoleNames.Any(roleName => restrictedRoles.Contains(roleName));

                // Check if request exists and is not deleted
                var requestExists = await _context.BaseRequests
                    .AnyAsync(br => br.Id == requestId && !br.IsDeleted, cancellationToken);
                
                if (!requestExists)
                    return APIOperationResponse<BaseRequestDto>.NotFound($"Request with ID {requestId} not found.");

                // If superadmin, check permission (still filter by department if user has restricted role)
                if (_currentUserService.IsSuperAdmin)
                {
                    var query = _context.BaseRequests.Where(br => br.Id == requestId && !br.IsDeleted);
                    
                    // Filter by department only if user has a restricted role
                    if (shouldFilterByDepartment)
                    {
                        query = query.Where(br => br.DepartmentId == userDepartmentId.Value);
                    }
                    
                    hasPermission = await query.AnyAsync(cancellationToken);
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
                        
                        hasWorkflowPermission = await workflowQuery.AnyAsync(cancellationToken);
                    }

                    // Check if user is the requester
                    var requesterQuery = _context.BaseRequests.Where(br => br.Id == requestId && !br.IsDeleted && br.RequesterId == currentUserId);
                    
                    // Filter by department only if user has a restricted role
                    if (shouldFilterByDepartment)
                    {
                        requesterQuery = requesterQuery.Where(br => br.DepartmentId == userDepartmentId.Value);
                    }
                    
                    var isRequester = await requesterQuery.AnyAsync(cancellationToken);

                    // User has permission if they can approve OR they are the requester
                    hasPermission = hasWorkflowPermission || isRequester;
                }

                if (!hasPermission)
                    return APIOperationResponse<BaseRequestDto>.NotFound($"Request with ID {requestId} not found or access denied.");

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
                    .FirstOrDefaultAsync(cancellationToken);

                if (baseRequest == null)
                    return APIOperationResponse<BaseRequestDto>.NotFound($"Request with ID {requestId} not found.");

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
                                                        Files = new List<FileUploadDto>()
                                                    }
                                                })
                                                .OrderBy(h => h.History.ChangedAt)
                                                .ToListAsync(cancellationToken);

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
                                             .ToListAsync(cancellationToken);

                var detailParallelWfStepIds = pendingStepsData.Select(p => p.WorkflowStepId).Distinct().ToList();
                var detailParallelRolesByWfStep = await BuildParallelRolesByStepIdsAsync(detailParallelWfStepIds, cancellationToken);
                var detailParallelRoleIdsByWfStep = await BuildParallelRoleIdsByStepIdsAsync(detailParallelWfStepIds, cancellationToken);

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
                        .ToListAsync(cancellationToken);

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
                    .ToDictionaryAsync(g => g.Key, g => g.FirstOrDefault(), cancellationToken);

                // Get ALL workflow approval steps for this request (extra fields for AutoRejected future-step ChangedAt)
                var allWorkflowApprovalSteps = await _context.WorkflowApprovalSteps
                    .Where(was => was.TargetRequestId == requestId)
                    .Select(was => new { was.Id, was.TargetRequestId, was.WorkflowStepId, was.Status, was.CreationDate })
                    .ToListAsync(cancellationToken);

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

                // Do not synthesize pending/future steps when the workflow has ended
                bool skipPendingAndFutureSteps =
                    baseRequest.Status == RequestStatus.Rejected ||
                    baseRequest.Status == RequestStatus.Cancelled ||
                    baseRequest.Status == RequestStatus.Approved ||
                    baseRequest.Status == RequestStatus.AutoRejected;

                if (!skipPendingAndFutureSteps && combinedHistory.Any(h =>
                    h.NewRequestStatus == RequestStatus.Rejected ||
                    h.NewRequestStatus == RequestStatus.Cancelled))
                {
                    skipPendingAndFutureSteps = true;
                }

                if (!skipPendingAndFutureSteps)
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
                                // Auto-rejected: pending loader skips the stuck row (no longer Current); use persisted step CreationDate for "pending from".
                                DateTime futureStepChangedAt = DateTime.MinValue;
                                if (baseRequest.Status == RequestStatus.AutoRejected)
                                {
                                    var autoRejectedForStep = allWorkflowApprovalSteps
                                        .Where(s => s.Status == RequestStatus.AutoRejected && s.WorkflowStepId == nextWorkflowStep.Id)
                                        .OrderByDescending(s => s.Id)
                                        .FirstOrDefault();
                                    if (autoRejectedForStep != null && autoRejectedForStep.CreationDate > DateTime.MinValue)
                                    {
                                        futureStepChangedAt = autoRejectedForStep.CreationDate;
                                    }
                                }

                                var futureStep = new ApprovalHistoryDto
                                {
                                    Id = 0,
                                    WorkflowApprovalStepId = 0,
                                    WorkflowStepId = nextWorkflowStep.Id,
                                    OldRequestStatus = RequestStatus.New,
                                    NewRequestStatus = RequestStatus.New,
                                    Comments = null,
                                    ChangedBy = null,
                                    ChangedAt = futureStepChangedAt,
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
                                    Files = new List<FileUploadDto>(),
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
                    .OrderBy(h => h.IsPending ? 1 : 0)
                    .ThenBy(h => h.StepOrder ?? int.MaxValue)
                    .ThenBy(h => h.ChangedAt == DateTime.MinValue ? DateTime.MaxValue : h.ChangedAt)                    
                    .ToList();

                if (baseRequest.RequestType == RequestType.Order)
                {
                    baseRequest.SupplyDate = await _context.Set<Order>().AsNoTracking()
                        .Where(o => o.Id == requestId)
                        .Select(o => o.SupplyDate)
                        .FirstOrDefaultAsync(cancellationToken);
                }

                return APIOperationResponse<BaseRequestDto>.Success(baseRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching base request by ID {RequestId}", request.RequestId);
                return APIOperationResponse<BaseRequestDto>.ServerError($"Processing failed: {ex.Message}");
            }
        }

        private async Task<Dictionary<long, List<WorkflowStepParallelRoleDto>>> BuildParallelRolesByStepIdsAsync(List<long> stepIds, CancellationToken cancellationToken)
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
                .ToListAsync(cancellationToken);
            return rows
                .GroupBy(x => x.WorkflowStepId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        private async Task<Dictionary<long, List<string>>> BuildParallelRoleIdsByStepIdsAsync(List<long> stepIds, CancellationToken cancellationToken)
        {
            if (stepIds == null || stepIds.Count == 0)
                return new Dictionary<long, List<string>>();
            var distinctIds = stepIds.Distinct().ToList();
            return await _context.WorkflowStepParallelRoles
                .Where(pr => distinctIds.Contains(pr.WorkflowStepId))
                .GroupBy(pr => pr.WorkflowStepId)
                .ToDictionaryAsync(g => g.Key, g => g.Select(x => x.RoleId).ToList(), cancellationToken);
        }
    }
}
