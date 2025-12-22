using System.Collections.Generic;

namespace Ettad.Inventory.Service.Monitoring.Dtos
{
    public class LowStockNotificationSettingsDto
    {
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> Users { get; set; } = new List<string>();
    }
}

