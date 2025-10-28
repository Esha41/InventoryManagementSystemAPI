using Moujam.Casiher.Comman.Base;

namespace Ettad.Data.Entities   
{
    public class InventoryDetail : BaseEntity<long>
    {
        public long ItemId { get; set; }

        public long InventoryId { get; set; }

        public long ItemQuantity { get; set; }

        public long CurrentQuantity { get; set; }
    }
}
