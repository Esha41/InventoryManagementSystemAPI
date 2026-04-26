using System.Threading.Tasks;

namespace Ettad.Inventory.Service.Monitoring.Interfaces
{
    public interface ILowStockMonitorBackgroundService
    {
        Task CheckAndNotifyAsync();
    }
}

