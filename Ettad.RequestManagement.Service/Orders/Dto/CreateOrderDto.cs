using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Orders.Dto
{
    public class CreateOrderDto
    {
        #region BaseRequest Properties
        public string OrderNo { get; set; }
        // RequestType is automatically set to 'Order' in the service
        public string Reason { get; set; }
        public RequestPriority Priority { get; set; }
        // Status is automatically set to 'New' in the service
        public string Notes { get; set; }
        public long RequestPurposeId { get; set; }
        #endregion

        #region Order-Specific Properties
        public bool IsFromAllowance { get; set; }
        public DateTime UsageDate { get; set; }
        public TimeOnly UsageTime { get; set; }
        public string UsagePurpose { get; set; }
        public int? AnnualDiscard { get; set; }
        public string UsageLocation { get; set; }
        public int? NumberOfOfficer { get; set; }
        public int? NumberOfOtherRank { get; set; }
        #endregion

        #region Request Items
        public List<CreateUpdateRequestItemDto> RequestItems { get; set; }
        #endregion
    }
}

