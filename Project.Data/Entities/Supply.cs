using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities   
{
    public class Supply : FullAuditEntity<long>
    {
        public long OrderId { get; set; }
        public DateTime? SupplyDate { get; set; }
        public long? ReceiverEmployeeId { get; set; }
        public SupplySubmissionStatus SubmissionStatus { get; set; }
        public SupplyFulfillmentStatus FulfillmentStatus { get; set; }
        public string? Notes { get; set; }

        #region Navigation Properties

        public Order Order { get; set; }
        public Employee? ReceiverEmployee { get; set; }
        public ICollection<SupplyDetail> SupplyDetails { get; set; }

        #endregion
    }
}
