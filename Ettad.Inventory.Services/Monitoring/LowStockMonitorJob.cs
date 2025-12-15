using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Ettad.Inventory.Service.Monitoring
{
    /// <summary>
    /// Static wrapper class for Hangfire job execution.
    /// Hangfire requires static methods for job execution.
    /// This method uses a static service provider to resolve services from DI.
    /// </summary>
    public static class LowStockMonitorJob
    {
        private static IServiceProvider? _serviceProvider;

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [AutomaticRetry(Attempts = 3)]
        public static void Execute()
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider is not available. Make sure LowStockMonitorJob.SetServiceProvider() is called during application startup.");
            }

            using var scope = _serviceProvider.CreateScope();
            var backgroundService = scope.ServiceProvider.GetRequiredService<ILowStockMonitorBackgroundService>();
            backgroundService.CheckAndNotifyAsync().GetAwaiter().GetResult();
        }
    }
}

