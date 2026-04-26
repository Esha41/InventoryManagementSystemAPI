using System.Threading.Tasks;
using Ettad.Inventory.Service.Monitoring.Interfaces;
using Hangfire;

namespace Ettad.Inventory.Service.Monitoring.Services
{
    /// <summary>
    /// Hangfire job class for low stock monitoring.
    /// This class uses Hangfire's built-in dependency injection - 
    /// Hangfire resolves ILowStockMonitorBackgroundService from DI at execution time.
    /// </summary>
    public class LowStockMonitorJob
    {
        private readonly ILowStockMonitorBackgroundService _backgroundService;

        public LowStockMonitorJob(ILowStockMonitorBackgroundService backgroundService)
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
