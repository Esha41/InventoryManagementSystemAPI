using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Moujam.Casiher.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Inventory : AuditEntity<long>
    {
        public int DepoId { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? RecievedDate {  get; set; }
                   
        public string Notes { get; set; }

        #region Navigation Properties

        public Depo Depo { get; set; }
        public ICollection<InventoryDetail> InventoryDetails { get; set; }
        
        #endregion
    }
}
