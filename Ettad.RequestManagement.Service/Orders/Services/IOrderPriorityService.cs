using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Orders.Services;

/// <summary>
/// Encapsulates order priority business rules.
/// Single source of truth for all priority calculations across the system.
/// </summary>
public interface IOrderPriorityService
{
    /// <summary>
    /// Calculates priority from usage start date (local calendar days; handles UTC input).
    /// Overdue or ≤7 days → VeryUrgent | ≤14 days → Urgent | else → Normal.
    /// </summary>
    RequestPriority CalculatePriorityFromUsageDate(DateTime usageDateFrom);

    /// <summary>
    /// Returns whole calendar days remaining until usage date.
    /// Negative value means the date has already passed.
    /// </summary>
    int GetDaysUntilUsageDate(DateTime usageDateFrom);
}
