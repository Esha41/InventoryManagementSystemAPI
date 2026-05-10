using Ettad.Workflows.Service.Monitoring.Dtos;

namespace Ettad.Workflows.Service.Monitoring;

/// <summary>Backward-compatible entry point for order-scoped countdown calls; prefer <see cref="IRequestAutoRejectCountdownService"/>.</summary>
public interface IOrderAutoRejectCountdownService
{
    /// <returns>Forbidden=true when the user may not view this request&apos;s countdown.</returns>
    Task<(bool Forbidden, RequestAutoRejectCountdownDto? Dto)> GetForRequestAsync(long requestId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RequestAutoRejectCountdownDto>> GetBulkAsync(IReadOnlyList<long> requestIds, CancellationToken cancellationToken = default);

    Task<RequestAutoRejectDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
}
