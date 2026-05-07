namespace Ettad.Inventory.Service.Monitoring.Interfaces
{
    public interface ICriticalStockMonitorBackgroundService
    {
        Task CheckAndNotifyAsync();
    }
}
