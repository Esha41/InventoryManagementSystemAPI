using Ettad.Inventory.Service.Monitoring.Interfaces;
using Hangfire;

namespace Ettad.Inventory.Service.Monitoring.BackgroundJobs
{
    /// <summary>
    /// Hangfire job class for Critical stock monitoring.
    /// This class uses Hangfire's built-in dependency injection - 
    /// Hangfire resolves ICriticalStockMonitorBackgroundService from DI at execution time.
    /// </summary>
    public class CriticalStockMonitorJob
    {
        private readonly ICriticalStockMonitorBackgroundService _backgroundService;

        public CriticalStockMonitorJob(ICriticalStockMonitorBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ExecuteAsync()
        {
            await _backgroundService.CheckAndNotifyAsync();
        }
    }
}
