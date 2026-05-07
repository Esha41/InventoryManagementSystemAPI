namespace Ettad.RequestManagement.Service.Orders.Dto
{
    public class CreateOrderDto
    {
        #region BaseRequest Properties
        public string? Reason { get; set; }
        public string? Notes { get; set; }
        public string RequestPurposeNotes { get; set; } = string.Empty;
        public long RequestPurposeId { get; set; }
        #endregion

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

        #region Request Items
        public List<CreateUpdateRequestItemDto> RequestItems { get; set; }
        #endregion
    }
}

