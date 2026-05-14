using Ettad.Data.Enums;

namespace Ettad.Data.Orders;

/// <summary>
/// Defines when order priority is recomputed from <see cref="Entities.Order.UsageDateFrom"/> vs read from persistence.
/// </summary>
public static class OrderPriorityLifecycle
{
    /// <summary>
    /// Statuses where priority is derived live from usage start date (DTO mapping and new creates).
    /// </summary>
    public static bool UsesLivePriorityFromUsageDate(RequestStatus status) =>
        status is RequestStatus.New
            or RequestStatus.UnderProcess
            or RequestStatus.ReturnedForReview;
}
