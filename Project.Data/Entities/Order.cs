namespace Ettad.Data.Entities
{
    public class Order : BaseRequest
    {
        public bool IsFromAllowance { get; set; }
        public DateTime UsageDate { get; set; }
        public TimeOnly UsageTime { get; set; }
        public string UsagePurpose { get; set; }
        public int? AnnualDiscard { get; set; } 
        public string UsageLocation { get; set; }
        public int? NumberOfOfficer { get; set; }
        public int? NumberOfOtherRank { get; set; } 
    }
}
