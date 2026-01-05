namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// DTO for submitting an asset supply for approval/completion
    /// </summary>
    public class SubmitAssetSupplyDto
    {
        /// <summary>
        /// Name of the person receiving the supply
        /// </summary>
        public string? ReceiverName { get; set; }

        /// <summary>
        /// Military ID of the receiver
        /// </summary>
        public string? ReceiverMilitaryId { get; set; }

        /// <summary>
        /// Rank ID of the receiver
        /// </summary>
        public long? ReceiverRankId { get; set; }

        /// <summary>
        /// Date when supply is being issued
        /// </summary>
        public DateTime SupplyDate { get; set; }

        /// <summary>
        /// Additional notes for the submission
        /// </summary>
        public string? Notes { get; set; }
    }
}

