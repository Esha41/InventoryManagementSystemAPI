# Notification System Usage Examples

## Entity-Specific Notifications

Use when you want to link a notification to a specific entity instance (Order, Ammunition, etc.)

### Example 1: Order Created Notification
```csharp
await _notificationHelperService.SendNotificationAsync(
    title: "New Order Created",
    message: $"Order #{orderId} has been created and requires your approval",
    entityType: "Order",
    entityId: orderId,
    userIds: new List<string> { approverUserId }
);
```

### Example 2: Ammunition Low Stock Notification
```csharp
await _notificationHelperService.SendNotificationAsync(
    title: "Low Stock Alert",
    message: $"Ammunition '{ammunitionName}' is running low",
    entityType: "Ammunition",
    entityId: ammunitionId,
    roleIds: new List<string> { inventoryManagerRoleId }
);
```

### Example 3: Workflow Approval Required
```csharp
await _notificationHelperService.SendNotificationAsync(
    title: "Approval Required",
    message: $"Request #{requestId} is pending your approval",
    entityType: "Request",
    entityId: requestId,
    userIds: approverUserIds
);
```

## System-Wide / Broadcast Notifications

Use when the notification is not tied to a specific entity (system announcements, general messages, etc.)

### Example 1: System Maintenance Notification
```csharp
await _notificationHelperService.SendNotificationAsync(
    title: "System Maintenance Scheduled",
    message: "The system will be under maintenance tonight from 10 PM to 2 AM",
    entityType: null,  // No specific entity
    entityId: null,    // No specific entity ID
    roleIds: new List<string> { allUsersRoleId }
);
```

### Example 2: Password Expiry Warning
```csharp
await _notificationHelperService.SendNotificationAsync(
    title: "Password Expiring Soon",
    message: "Your password will expire in 7 days. Please update it soon.",
    entityType: null,
    entityId: null,
    userIds: new List<string> { userId }
);
```

### Example 3: New Feature Announcement
```csharp
await _notificationHelperService.SendNotificationAsync(
    title: "New Feature Available",
    message: "We've added a new reporting dashboard. Check it out!",
    entityType: null,
    entityId: null,
    roleIds: new List<string> { allUsersRoleId }
);
```

## Integration Examples

### In OrderService after creating an order:
```csharp
public async Task<APIOperationResponse<long>> CreateAsync(CreateOrderDto dto)
{
    // ... create order logic ...
    var orderId = createdOrder.Id;
    
    // Send notification to approvers
    await _notificationHelperService.SendNotificationAsync(
        title: "New Order Requires Approval",
        message: $"Order #{orderId} has been created and requires your review",
        entityType: "Order",
        entityId: orderId,
        roleIds: new List<string> { approverRoleId }
    );
    
    return APIOperationResponse<long>.Success(orderId);
}
```

### In AmmunitionService after updating stock:
```csharp
public async Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateAmmunitionDto dto)
{
    // ... update logic ...
    
    if (dto.Quantity < threshold)
    {
        await _notificationHelperService.SendNotificationAsync(
            title: "Low Stock Alert",
            message: $"Ammunition '{ammunition.Name}' quantity is below threshold",
            entityType: "Ammunition",
            entityId: id,
            roleIds: new List<string> { inventoryManagerRoleId }
        );
    }
    
    return APIOperationResponse<bool>.Success(true);
}
```

## Validation Rules

- **Entity-Specific**: If `EntityType` is provided, `EntityId` must also be provided (and > 0)
- **System Notifications**: Both `EntityType` and `EntityId` can be null
- **Receivers**: At least one `UserId` or `RoleId` must be specified
- **Title & Message**: Always required

## Frontend Usage

When displaying notifications, check if `EntityType` and `EntityId` exist to determine if it's clickable:

```javascript
if (notification.entityType && notification.entityId) {
    // Entity-specific notification - make it clickable
    // Navigate to: `/entity/${notification.entityType}/${notification.entityId}`
} else {
    // System notification - display as informational only
}
```

