using Ettad.Workflows.Service.Monitoring.Dtos;

namespace Ettad.Workflows.Service.Monitoring;

public interface IOrderAutoRejectCountdownService
{
    /// <returns>Forbidden=true when the user may not view this request&apos;s countdown.</returns>
    Task<(bool Forbidden, OrderAutoRejectCountdownDto? Dto)> GetForRequestAsync(long requestId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OrderAutoRejectCountdownDto>> GetBulkAsync(IReadOnlyList<long> requestIds, CancellationToken cancellationToken = default);

    Task<OrderAutoRejectDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
}
