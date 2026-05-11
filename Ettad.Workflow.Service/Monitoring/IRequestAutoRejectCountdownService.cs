using Ettad.Workflows.Service.Monitoring.Dtos;

namespace Ettad.Workflows.Service.Monitoring;

public interface IRequestAutoRejectCountdownService
{
    /// <returns>Forbidden=true when the user may not view this request&apos;s countdown.</returns>
    Task<(bool Forbidden, RequestAutoRejectCountdownDto? Dto)> GetCountdownAsync(long requestId, string requestType, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RequestAutoRejectCountdownDto>> GetBulkAsync(IReadOnlyList<long> requestIds, string requestType, CancellationToken cancellationToken = default);

    Task<RequestAutoRejectDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
}
