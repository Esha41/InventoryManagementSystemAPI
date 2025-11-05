using Ettad.Data.Entities;
using Ettad.Data.Enums;


namespace Ettad.RequestManagement.Service.Orders.Dto
{
    public class OrderDto
    {
        public long Id { get; set; }

        #region BaseRequest Properties
        public string OrderNo { get; set; }
        public RequestType RequestType { get; set; }
        public string Reason { get; set; }
        public RequestPriority Priority { get; set; }
        public RequestStatus Status { get; set; }
        public string Notes { get; set; }
        public long DepartmentId { get; set; }
        public long? RequesterId { get; set; }
        public long? RecieverId { get; set; }
        public long? DepotId { get; set; }
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

        

        #region Navigation Properties
        public Department Department { get; set; }
        public Employee Requester { get; set; }
        public Employee Reciever { get; set; }
        public Depot Depot { get; set; }
        public RequestPurpose RequestPurpose { get; set; }
        public ICollection<RequestItem> RequestItems { get; set; }
        #endregion
    }
}
