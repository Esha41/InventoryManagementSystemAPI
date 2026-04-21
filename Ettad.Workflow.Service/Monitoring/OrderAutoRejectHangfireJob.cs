using Hangfire;

namespace Ettad.Workflows.Service.Monitoring;

public class OrderAutoRejectHangfireJob
{
    private readonly IOrderAutoRejectBackgroundService _backgroundService;

    public OrderAutoRejectHangfireJob(IOrderAutoRejectBackgroundService backgroundService)
    {
        _backgroundService = backgroundService;
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task ExecuteAsync()
    {
        await _backgroundService.ScanAsync();
    }
}
