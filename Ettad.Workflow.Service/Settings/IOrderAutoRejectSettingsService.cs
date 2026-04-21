using Ettad.Workflows.Service.Settings.Dtos;

namespace Ettad.Workflows.Service.Settings;

public interface IOrderAutoRejectSettingsService
{
    Task<OrderAutoRejectSettingsDto> GetAsync(CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateOrderAutoRejectSettingsDto dto, CancellationToken cancellationToken = default);
}
