using Ettad.Data.Entities.Workflows;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Workflows.Service.DTO;
using Ettad.Workflows.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Application.Common.Interfaces;

namespace Ettad.Workflows.Service.Imeplemention
{
    public class WorkflowApprovalService:IWorkflowApprovalService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public WorkflowApprovalService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
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
                ApprovedDate = dto.ApprovedDate,
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
            // 1. Get current user's roles from database
            var userRoleIds = await _context.UserRoles
                .Where(ur => ur.UserId == _currentUserService.UserId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            // 2. Query workflow approval steps joined with workflow steps and base requests
            var query = from ws in _context.WorkflowApprovalSteps
                        join br in _context.BaseRequests
                            on ws.TargetRequestId equals br.Id
                        join wfs in _context.WorkflowSteps
                            on ws.WorkflowStepId equals wfs.Id
                        // Filter workflow steps where the user has permission
                        where userRoleIds.Contains(wfs.ApplicationRoleId)
                           || (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId) && userRoleIds.Contains(wfs.HigherApprovalRoleId))
                        select new WorkflowApprovalWithOrderDto
                        {
                            // WorkflowStep properties (wfs)
                            WorkflowId = wfs.WorkflowId,
                            StepOrder = wfs.StepOrder,
                            ApplicationRoleId = wfs.ApplicationRoleId,
                            ApplicationEntityId = wfs.ApplicationEntityId,
                            RequireHigherApproval = wfs.RequireHigherApproval,
                            HigherApprovalRoleId = wfs.HigherApprovalRoleId,
                            HigherApplicationEntityId = wfs.HigherApplicationEntityId,
                            ReserveQty = wfs.ReserveQty,
                            // WorkflowApprovalStep properties (ws)
                            WorkflowStepId = ws.WorkflowStepId,
                            TargetRequestId = ws.TargetRequestId,
                            RequestType = ws.RequestType,
                            ApprovedDate = ws.ApprovedDate,
                            Status = ws.Status,
                            Comments = ws.Comments,
                            IsCurrent = ws.IsCurrent,
                            ApproverUserId = ws.ApproverUserId,
                            // BaseRequest properties (br)
                            BaseRequestId = br.Id,
                            RequestNo = br.RequestNo,
                            BaseRequestType = br.RequestType,
                            Reason = br.Reason,
                            Priority = br.Priority,
                            BaseRequestStatus = br.Status,
                            Notes = br.Notes,
                            DepartmentId = br.DepartmentId,
                            RequesterId = br.RequesterId,
                            RecieverId = br.RecieverId,
                            DepotId = br.DepotId,
                            RequestPurposeId = br.RequestPurposeId,
                            WorkflowApprovalStepId = ws.Id,
                        };

            return await query.ToListAsync();
        }


        public async Task<WorkflowApprovalStepDto> ApproveOrRejectAsync(ApproveRejectWorkflowApprovalDto dto)
        {
            try
            {
                // 1. Load approval step including workflow step
                var approvalStep = await _context.WorkflowApprovalSteps
                    .Include(x => x.WorkflowStep)
                    .FirstOrDefaultAsync(x => x.Id == dto.WorkflowApprovalStepId);

                if (approvalStep == null)
                    throw new KeyNotFoundException($"WorkflowApprovalStep with Id {dto.WorkflowApprovalStepId} not found");

                if (approvalStep.Status != RequestStatus.New && approvalStep.Status != RequestStatus.UnderProcess)
                    throw new InvalidOperationException($"WorkflowApprovalStep with Id {dto.WorkflowApprovalStepId} has already been processed");

                // 2. Get user roles
                var userRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == _currentUserService.UserId)
                    .Select(ur => ur.RoleId)
                    .ToListAsync();

                // 3. Permission check
                if (!_currentUserService.IsSuperAdmin && approvalStep.WorkflowStep != null)
                {
                    var step = approvalStep.WorkflowStep;

                    var allowedRoles = new List<string>
            {
                step.ApplicationRoleId
            };

                    if (!string.IsNullOrEmpty(step.HigherApprovalRoleId))
                        allowedRoles.Add(step.HigherApprovalRoleId);

                    bool hasPermission = userRoles.Any(role => allowedRoles.Contains(role));

                    if (!hasPermission)
                        throw new UnauthorizedAccessException("User does not have permission to approve/reject this workflow step.");
                }

                // Save old status for log
                var oldStatus = approvalStep.Status;

                // 4. Approve or reject the step
                approvalStep.Status = dto.IsApproved ? RequestStatus.Approved : RequestStatus.Rejected;
                approvalStep.ApprovedDate = DateTime.UtcNow;
                approvalStep.ApproverUserId = _currentUserService.UserId;
                approvalStep.Comments = dto.Comments;
                approvalStep.IsCurrent = false;
                approvalStep.ModificationDate = DateTime.UtcNow;
                approvalStep.ModifiedBy = _currentUserService.UserId;

                // 5. Insert log
                var logEntry = new WorkflowStepApprovalLog
                {
                    WorkflowApprovalStepId = approvalStep.Id,                  
                    OldRequestStatus = oldStatus,
                    NewRequestStatus = approvalStep.Status,
                    Comments = dto.Comments,
                    ChangedBy = _currentUserService.UserId,
                    ChangedAt = DateTime.UtcNow,
                    CreationDate = DateTime.UtcNow,
                    CreatedBy = _currentUserService.UserId,
                    ModificationDate = DateTime.UtcNow,
                    ModifiedBy = _currentUserService.UserId
                };

                _context.WorkflowStepApprovalLog.Add(logEntry);

                // 6. Update Base Request
                var baseRequest = await _context.BaseRequests
                    .FirstOrDefaultAsync(x => x.Id == approvalStep.TargetRequestId);

                if (baseRequest != null)
                {
                    if (dto.IsApproved)
                    {
                        // Get ordered workflow steps
                        var workflowSteps = await _context.WorkflowSteps
                            .Where(ws => ws.WorkflowId == approvalStep.WorkflowStep.WorkflowId)
                            .OrderBy(ws => ws.StepOrder)
                            .ToListAsync();

                        var currentOrder = approvalStep.WorkflowStep.StepOrder;
                        var nextStep = workflowSteps.FirstOrDefault(ws => ws.StepOrder > currentOrder);

                        if (nextStep != null)
                        {
                            // Create next step
                            var nextApproval = new WorkflowApprovalStep
                            {
                                WorkflowStepId = nextStep.Id,
                                TargetRequestId = approvalStep.TargetRequestId,
                                RequestType = approvalStep.RequestType,
                                Status = RequestStatus.New,
                                IsCurrent = true,
                                CreationDate = DateTime.UtcNow,
                                CreatedBy = _currentUserService.UserId
                            };

                            _context.WorkflowApprovalSteps.Add(nextApproval);

                            baseRequest.Status = RequestStatus.UnderProcess;
                        }
                        else
                        {
                            baseRequest.Status = RequestStatus.Approved;
                        }
                    }
                    else
                    {
                        // Reject case
                        baseRequest.Status = RequestStatus.Rejected;

                        var otherSteps = await _context.WorkflowApprovalSteps
                            .Where(x => x.TargetRequestId == approvalStep.TargetRequestId &&
                                        x.IsCurrent &&
                                        (x.Status == RequestStatus.New || x.Status == RequestStatus.UnderProcess) &&
                                        x.Id != approvalStep.Id)
                            .ToListAsync();

                        foreach (var step in otherSteps)
                        {
                            step.IsCurrent = false;
                            step.ModifiedBy = _currentUserService.UserId;
                            step.ModificationDate = DateTime.UtcNow;
                        }
                    }

                    baseRequest.ModifiedBy = _currentUserService.UserId;
                    baseRequest.ModificationDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                // 7. Return DTO
                return new WorkflowApprovalStepDto
                {
                    Id = approvalStep.Id,
                    WorkflowStepId = approvalStep.WorkflowStepId,
                    TargetRequestId = approvalStep.TargetRequestId,
                    RequestType = approvalStep.RequestType,
                    ApproverUserId = approvalStep.ApproverUserId,
                    IsDelegation = approvalStep.IsDelegation,
                    ApprovedDate = approvalStep.ApprovedDate,
                    Status = approvalStep.Status,
                    Comments = approvalStep.Comments,
                    IsCurrent = approvalStep.IsCurrent,
                    ChangedBy = approvalStep.ModifiedBy
                };
            }
            catch (Exception)
            {
                // Re-throw so controller ProcessResponse() handles it cleanly
                throw;
            }
        }

    }
}
