namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// DTO for updating an existing asset supply (only while in Draft status)
    /// </summary>
    public class UpdateAssetSupplyDto
    {
        /// <summary>
        /// Optional custodian ID to assign assets to
        /// </summary>
        public long? CustodianId { get; set; }

        /// <summary>
        /// Optional department ID override
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        /// Location where assets will be assigned
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Expected return date for the assets
        /// </summary>
        public DateTime? ExpectedReturnDate { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Updated list of assets (replaces existing)
        /// </summary>
        public List<CreateAssetSupplyDetailDto>? SupplyDetails { get; set; }
    }
}

