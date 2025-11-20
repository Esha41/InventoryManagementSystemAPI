using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Inventories.Dtos
{
    public class InventoryDto
    {
        public long Id { get; set; }

        public long DepoId { get; set; }

        public string? InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? RecievedDate { get; set; }

        public string Notes { get; set; }

        #region Navigation Properties

        public DepotDto Depo { get; set; }
        public List<InventoryDetailDto> InventoryDetails { get; set; }

        #endregion
    }
}
