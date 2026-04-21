namespace Ettad.Workflows.Service.Monitoring;

public interface IOrderAutoRejectBackgroundService
{
    Task ScanAsync(CancellationToken cancellationToken = default);
}
