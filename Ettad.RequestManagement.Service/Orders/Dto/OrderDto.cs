using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Common.Dtos;

namespace Ettad.RequestManagement.Service.Orders.Dto
{
    public class OrderDto : BaseRequestDto
    {
        #region Order-Specific Properties
        public bool IsFromAllowance { get; set; }
        public DateTime UsageDateFrom { get; set; }
        public TimeOnly UsageTimeFrom { get; set; }
        public DateTime UsageDateTo { get; set; }
        public TimeOnly UsageTimeTo { get; set; }
        public string UsagePurpose { get; set; }
        public int? AnnualDiscard { get; set; }
        public string UsageLocation { get; set; }
        public int? NumberOfOfficer { get; set; }
        public int? NumberOfOtherRank { get; set; }
        #endregion

        //#region Navigation Names (Order-Specific - Flattened for convenience)
        //public string DepartmentNameAr { get; set; }
        //public string DepartmentNameEn { get; set; }
        //public string RequesterName { get; set; }
        //public string RequestPurposeNameAr { get; set; }
        //public string RequestPurposeNameEn { get; set; }
        
        //// Override to use Order-specific item DTO with ItemType
        //public new ICollection<OrderRequestItemDto> RequestItems { get; set; }
        //#endregion
    }
}
