using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Inventory : FullAuditEntity<long>
    {
        public long DepoId { get; set; }

        public string? InvoiceNumber { get; set; }

        public string? DeliveryReceipt { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? RecievedDate {  get; set; }

        public string? ContractNumber { get; set; }
                   
        public string Notes { get; set; }

        #region Navigation Properties

        public Depot Depo { get; set; }
        public ICollection<InventoryDetail> InventoryDetails { get; set; }
        
        #endregion
    }
}
