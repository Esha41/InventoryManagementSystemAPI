using Ettad.Data.Entities;

namespace Ettad.Inventory.Service.Inventories.Dtos
{
    public class InventoryDto
    {
        public long Id { get; set; }

        public long DepoId { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? RecievedDate { get; set; }

        public string Notes { get; set; }

        #region Navigation Properties

        public Depot Depo { get; set; }
        public List<InventoryDetailDto> InventoryDetails { get; set; }

        #endregion
    }
}
