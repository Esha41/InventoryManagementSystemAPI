using System.Threading.Tasks;

namespace Ettad.Inventory.Service.Monitoring
{
    public interface ILowStockMonitorBackgroundService
    {
        Task CheckAndNotifyAsync();
    }
}

