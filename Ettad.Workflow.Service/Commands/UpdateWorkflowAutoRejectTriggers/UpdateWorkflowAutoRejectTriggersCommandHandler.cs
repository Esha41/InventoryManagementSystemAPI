using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Dtos;
using Ettad.Workflows.Service.Monitoring;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Commands.UpdateWorkflowAutoRejectTriggers;

public sealed class UpdateWorkflowAutoRejectTriggersCommandHandler
    : IRequestHandler<UpdateWorkflowAutoRejectTriggersCommand, APIOperationResponse<WorkflowAutoRejectTriggerDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IWorkflowAutoRejectConfigCache _configCache;
    private readonly ILogger<UpdateWorkflowAutoRejectTriggersCommandHandler> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;

    public UpdateWorkflowAutoRejectTriggersCommandHandler(
        ApplicationDbContext context,
        IWorkflowAutoRejectConfigCache configCache,
        ILogger<UpdateWorkflowAutoRejectTriggersCommandHandler> logger,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _configCache = configCache;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
    }

    public async Task<APIOperationResponse<WorkflowAutoRejectTriggerDto>> Handle(
        UpdateWorkflowAutoRejectTriggersCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error updating auto-reject triggers for workflow {WorkflowId}", request.WorkflowId);
            return APIOperationResponse<WorkflowAutoRejectTriggerDto>.ServerError("Failed to update triggers");
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating auto-reject triggers for workflow {WorkflowId}", request.WorkflowId);
            return APIOperationResponse<WorkflowAutoRejectTriggerDto>.ServerError("Failed to update triggers");
        }
    }

    private async Task<APIOperationResponse<WorkflowAutoRejectTriggerDto>> ExecuteAsync(
        UpdateWorkflowAutoRejectTriggersCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Update auto-reject triggers for workflow {WorkflowId}", request.WorkflowId);

        var workflowExists = await _context.Workflows.AsNoTracking()
            .AnyAsync(w => w.Id == request.WorkflowId && !w.IsDeleted, cancellationToken);
        if (!workflowExists)
        {
            return APIOperationResponse<WorkflowAutoRejectTriggerDto>.Fail(ResponseType.NotFound, "Workflow not found.");
        }

        if (!request.Mode.HasValue)
            return APIOperationResponse<WorkflowAutoRejectTriggerDto>.BadRequest("Mode is required");

        var mode = request.Mode.Value;

        if (mode == AutoRejectTriggerMode.None)
            return await ClearAndRespondAsync(request.WorkflowId, cancellationToken).ConfigureAwait(false);

        if (mode == AutoRejectTriggerMode.Role)
        {
            var roles = NormalizeRoleIds(request.TriggerRoleIds);
            if (roles.Count == 0)
            {
                return APIOperationResponse<WorkflowAutoRejectTriggerDto>.BadRequest(
                    "At least one role must be selected for role-based triggers");
            }

            var rolesSet = roles.ToHashSet(StringComparer.Ordinal);
            var foundRoleCount = await _context.Roles.AsNoTracking()
                .Where(r => rolesSet.Contains(r.Id))
                .Select(r => r.Id)
                .CountAsync(cancellationToken);
            if (foundRoleCount != rolesSet.Count)
            {
                return APIOperationResponse<WorkflowAutoRejectTriggerDto>.BadRequest(
                    "One or more selected roles are invalid.");
            }
        }
        else if (mode == AutoRejectTriggerMode.Step)
        {
            var stepIds = NormalizeStepIds(request.TriggerStepIds);
            if (stepIds.Count == 0)
            {
                return APIOperationResponse<WorkflowAutoRejectTriggerDto>.BadRequest(
                    "At least one step must be selected for step-based triggers");
            }

            var validIds = (await _context.WorkflowSteps.AsNoTracking()
                .Where(s => s.WorkflowId == request.WorkflowId)
                .Select(s => s.Id)
                .ToListAsync(cancellationToken).ConfigureAwait(false)).ToHashSet();

            if (stepIds.Any(id => !validIds.Contains(id)))
            {
                return APIOperationResponse<WorkflowAutoRejectTriggerDto>.BadRequest(
                    "One or more selected steps are invalid for this workflow.");
            }
        }
        else if (mode != AutoRejectTriggerMode.Disabled)
        {
            return APIOperationResponse<WorkflowAutoRejectTriggerDto>.BadRequest("Invalid trigger mode");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var now = _dateTimeProvider.Now;
            var user = _currentUserService.UserName ?? string.Empty;

            var triggerEntity = await _context.WorkflowAutoRejectTriggers
                .Include(t => t.TriggerRoles)
                .Include(t => t.TriggerSteps)
                .FirstOrDefaultAsync(t => t.WorkflowId == request.WorkflowId, cancellationToken)
                .ConfigureAwait(false);

            if (triggerEntity != null)
            {
                triggerEntity.Mode = mode;
                triggerEntity.ModifiedBy = user;
                triggerEntity.ModificationDate = now;
                triggerEntity.TriggerRoles.Clear();
                triggerEntity.TriggerSteps.Clear();
            }
            else
            {
                triggerEntity = new Ettad.Data.Entities.Workflows.WorkflowAutoRejectTrigger
                {
                    WorkflowId = request.WorkflowId,
                    Mode = mode,
                    CreatedBy = user,
                    CreationDate = now,
                    ModifiedBy = user,
                    ModificationDate = now
                };
                await _context.WorkflowAutoRejectTriggers.AddAsync(triggerEntity, cancellationToken).ConfigureAwait(false);
            }

            if (mode == AutoRejectTriggerMode.Role)
            {
                foreach (var rid in NormalizeRoleIds(request.TriggerRoleIds))
                {
                    triggerEntity.TriggerRoles.Add(new Ettad.Data.Entities.Workflows.WorkflowAutoRejectTriggerRole
                    {
                        RoleId = rid,
                        CreatedBy = user,
                        CreationDate = now,
                        ModifiedBy = user,
                        ModificationDate = now
                    });
                }
            }
            else if (mode == AutoRejectTriggerMode.Step)
            {
                foreach (var sid in NormalizeStepIds(request.TriggerStepIds))
                {
                    triggerEntity.TriggerSteps.Add(new Ettad.Data.Entities.Workflows.WorkflowAutoRejectTriggerStep
                    {
                        WorkflowStepId = sid,
                        CreatedBy = user,
                        CreationDate = now,
                        ModifiedBy = user,
                        ModificationDate = now
                    });
                }
            }

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

            _configCache.InvalidateConfig(request.WorkflowId);
            _logger.LogInformation(
                "Auto-reject triggers updated for workflow {WorkflowId}: mode={Mode}",
                request.WorkflowId,
                triggerEntity.Mode);

            return APIOperationResponse<WorkflowAutoRejectTriggerDto>.Success(ToResponse(triggerEntity));
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    private async Task<APIOperationResponse<WorkflowAutoRejectTriggerDto>> ClearAndRespondAsync(
        long workflowId,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var existing = await _context.WorkflowAutoRejectTriggers
                .Include(t => t.TriggerRoles)
                .Include(t => t.TriggerSteps)
                .FirstOrDefaultAsync(t => t.WorkflowId == workflowId, cancellationToken)
                .ConfigureAwait(false);

            if (existing != null)
                _context.WorkflowAutoRejectTriggers.Remove(existing);

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            _configCache.InvalidateConfig(workflowId);

            return APIOperationResponse<WorkflowAutoRejectTriggerDto>.Success(
                new WorkflowAutoRejectTriggerDto
                {
                    Id = 0,
                    WorkflowId = workflowId,
                    Mode = null,
                    TriggerRoleIds = [],
                    TriggerStepIds = []
                });
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    private static List<string> NormalizeRoleIds(IEnumerable<string>? ids) =>
        (ids ?? Enumerable.Empty<string>())
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .Select(r => r.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToList();

    private static List<long> NormalizeStepIds(IEnumerable<long>? ids) =>
        (ids ?? Enumerable.Empty<long>()).Where(id => id > 0).Distinct().ToList();

    private static string? ToResponseModeString(AutoRejectTriggerMode mode) =>
        mode switch
        {
            AutoRejectTriggerMode.Role => "Role",
            AutoRejectTriggerMode.Step => "Step",
            AutoRejectTriggerMode.Disabled => "Disabled",
            AutoRejectTriggerMode.None => "None",
            _ => null
        };

    private static WorkflowAutoRejectTriggerDto ToResponse(Ettad.Data.Entities.Workflows.WorkflowAutoRejectTrigger entity) =>
        new()
        {
            Id = entity.Id,
            WorkflowId = entity.WorkflowId,
            Mode = ToResponseModeString(entity.Mode),
            TriggerRoleIds = entity.TriggerRoles
                .Select(tr => tr.RoleId)
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList(),
            TriggerStepIds = entity.TriggerSteps
                .Select(ts => ts.WorkflowStepId)
                .Where(s => s > 0)
                .ToList()
        };
}
