# Workflow Step Notifier Service - Usage Guide

## Overview

The `WorkflowStepNotifierService` allows you to configure which users and roles should be notified when actions are taken on workflow steps. Each workflow step can have multiple notifiers (users and/or roles).

## Database Schema

### WorkflowStepNotifier Entity

- **Id**: Primary key
- **WorkflowStepId**: Foreign key to WorkflowStep (required)
- **UserId**: Foreign key to ApplicationUser (nullable)
- **RoleId**: Foreign key to ApplicationRole (nullable)
- **Constraint**: Either UserId OR RoleId must be set (not both, not neither)

## Service Methods

### 1. Get Notifiers for a Step

```csharp
var result = await _workflowStepNotifierService.GetNotifiersByStepIdAsync(workflowStepId);
if (result.Succeeded)
{
    var notifiers = result.Data;
    foreach (var notifier in notifiers)
    {
        if (notifier.UserId != null)
        {
            Console.WriteLine($"User: {notifier.UserName} ({notifier.UserFullNameEn})");
        }
        else if (notifier.RoleId != null)
        {
            Console.WriteLine($"Role: {notifier.RoleName} ({notifier.RoleNameAr})");
        }
    }
}
```

### 2. Update All Notifiers for a Step

This replaces all existing notifiers with the new list:

```csharp
var updateDto = new UpdateWorkflowStepNotifiersDto
{
    WorkflowStepId = 1,
    UserIds = new List<string> { "user-id-1", "user-id-2" },
    RoleIds = new List<string> { "role-id-1" }
};

var result = await _workflowStepNotifierService.UpdateStepNotifiersAsync(updateDto);
if (result.Succeeded)
{
    Console.WriteLine("Notifiers updated successfully");
}
```

### 3. Add Notifiers (Supports Multiple)

```csharp
// Add multiple user and role notifiers at once
var addDto = new CreateWorkflowStepNotifierDto
{
    WorkflowStepId = 1,
    UserIds = new List<string> { "user-id-1", "user-id-2" },
    RoleIds = new List<string> { "role-id-1" }
};

var success = await _workflowStepNotifierService.AddNotifierAsync(addDto);
if (success)
{
    Console.WriteLine("Notifiers added successfully");
}
else
{
    Console.WriteLine("Failed to add notifiers");
}

// You can also add only users or only roles
var addUsersOnlyDto = new CreateWorkflowStepNotifierDto
{
    WorkflowStepId = 1,
    UserIds = new List<string> { "user-id-1", "user-id-2" }
};

var addRolesOnlyDto = new CreateWorkflowStepNotifierDto
{
    WorkflowStepId = 1,
    RoleIds = new List<string> { "role-id-1", "role-id-2" }
};
```

### 4. Remove a Notifier

```csharp
var result = await _workflowStepNotifierService.RemoveNotifierAsync(notifierId);
```

### 5. Get Notifier IDs (for sending notifications)

```csharp
var result = await _workflowStepNotifierService.GetNotifierIdsByStepIdAsync(workflowStepId);
if (result.Succeeded)
{
    var (userIds, roleIds) = result.Data;
    
    // Use these IDs with NotificationHelperService
    await _notificationHelperService.SendNotificationAsync(
        title: "Workflow Step Action Taken",
        message: $"An action was taken on workflow step {workflowStepId}",
        entityType: "WorkflowStep",
        entityId: workflowStepId,
        userIds: userIds,
        roleIds: roleIds
    );
}
```

## Integration Example: Sending Notifications on Workflow Actions

Here's how to integrate the notifier service into your workflow approval process:

```csharp
public async Task<APIOperationResponse<bool>> ProcessActionAsync(ApproveRejectWorkflowApprovalDto model)
{
    await using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        // Get current step
        var currentStep = await GetCurrentApprovalStepByRequestIdAsync(model.BaseRequestID);
        if (currentStep == null)
            return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "No current workflow step found.");

        var oldStatus = currentStep.Status;
        currentStep.Comments = model.Comments;

        // Process the action
        switch (model.Action)
        {
            case RequestStatus.Approved:
                await ApproveStepAsync(currentStep, model);
                break;
            case RequestStatus.Rejected:
                await RejectStepAsync(currentStep, model);
                break;
            default:
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Invalid workflow action.");
        }

        // Log action
        await LogStepActionAsync(currentStep.Id, currentStep.WorkflowStepId, oldStatus, model.Action, model.Comments, _currentUserService.UserName);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        // 🔔 Send notifications to step notifiers
        await SendNotificationsToStepNotifiersAsync(currentStep.WorkflowStepId, model);

        return APIOperationResponse<bool>.Success(true);
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}

private async Task SendNotificationsToStepNotifiersAsync(int workflowStepId, ApproveRejectWorkflowApprovalDto model)
{
    try
    {
        // Get notifiers for this step
        var notifiersResult = await _workflowStepNotifierService.GetNotifierIdsByStepIdAsync(workflowStepId);
        
        if (!notifiersResult.Succeeded || 
            (notifiersResult.Data.UserIds.Count == 0 && notifiersResult.Data.RoleIds.Count == 0))
        {
            // No notifiers configured for this step
            return;
        }

        var (userIds, roleIds) = notifiersResult.Data;

        // Determine action message
        var actionMessage = model.Action switch
        {
            RequestStatus.Approved => "approved",
            RequestStatus.Rejected => "rejected",
            RequestStatus.Returned => "returned",
            _ => "updated"
        };

        // Send notification
        await _notificationHelperService.SendNotificationAsync(
            title: $"Workflow Step {actionMessage}",
            message: $"Request #{model.BaseRequestID} has been {actionMessage}" + 
                     (string.IsNullOrEmpty(model.Comments) ? "" : $". Comments: {model.Comments}"),
            entityType: "WorkflowApprovalStep",
            entityId: model.BaseRequestID,
            userIds: userIds.Any() ? userIds : null,
            roleIds: roleIds.Any() ? roleIds : null,
            senderId: _currentUserService.UserId
        );
    }
    catch (Exception ex)
    {
        // Log error but don't fail the workflow action
        _logger.LogError(ex, "Error sending notifications to step notifiers for step {WorkflowStepId}", workflowStepId);
    }
}
```

## API Controller Example

```csharp
[ApiController]
[Route("api/[controller]")]
public class WorkflowStepNotifierController : ControllerBase
{
    private readonly IWorkflowStepNotifierService _notifierService;

    public WorkflowStepNotifierController(IWorkflowStepNotifierService notifierService)
    {
        _notifierService = notifierService;
    }

    [HttpGet("step/{workflowStepId}")]
    public async Task<ActionResult<List<WorkflowStepNotifierDto>>> GetNotifiersByStepId(int workflowStepId)
    {
        var result = await _notifierService.GetNotifiersByStepIdAsync(workflowStepId);
        if (!result.Succeeded)
            return BadRequest(result.Message);
        
        return Ok(result.Data);
    }

    [HttpPut("step/{workflowStepId}")]
    public async Task<ActionResult<bool>> UpdateStepNotifiers(int workflowStepId, [FromBody] UpdateWorkflowStepNotifiersDto dto)
    {
        if (dto.WorkflowStepId != workflowStepId)
            return BadRequest("WorkflowStepId mismatch");

        var result = await _notifierService.UpdateStepNotifiersAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<ActionResult<bool>> AddNotifiers([FromBody] CreateWorkflowStepNotifierDto dto)
    {
        var success = await _notifierService.AddNotifierAsync(dto);
        if (!success)
            return BadRequest("Failed to add notifiers. Please check that the workflow step exists and all user/role IDs are valid.");

        return Ok(success);
    }

    [HttpDelete("{notifierId}")]
    public async Task<ActionResult<bool>> RemoveNotifier(int notifierId)
    {
        var result = await _notifierService.RemoveNotifierAsync(notifierId);
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }
}
```

## Migration

After creating the entity and configuration, you'll need to create and apply a migration:

```bash
# In Package Manager Console or CLI
dotnet ef migrations add AddWorkflowStepNotifier --project Project.EntityFramework --startup-project Project.Api
dotnet ef database update --project Project.EntityFramework --startup-project Project.Api
```

## Notes

1. **Validation**: The service validates that users and roles exist before adding them as notifiers
2. **Duplicates**: The service prevents duplicate entries (same user/role for the same step)
3. **Cascade Delete**: When a workflow step is deleted, all its notifiers are automatically deleted
4. **Performance**: Indexes are created on WorkflowStepId, UserId, and RoleId for fast lookups
5. **Logging**: All operations are logged for audit purposes

