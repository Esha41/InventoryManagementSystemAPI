namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// DTO for creating and submitting a new asset supply in one step
    /// </summary>
    public class CreateAssetSupplyDto
    {
        /// <summary>
        /// The order ID this supply fulfills
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Optional custodian user ID (AspNet user) at supply header level.
        /// Detail-level and assignments use Employee IDs.
        /// </summary>
        // public string? CustodianId { get; set; }

        /// <summary>
        /// Employee receiving the supply (Required)
        /// </summary>
        public long ReceiverEmployeeId { get; set; }

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
        /// List of assets to include in this supply
        /// </summary>
        public List<CreateAssetSupplyDetailDto> SupplyDetails { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating an asset supply detail
    /// </summary>
    public class CreateAssetSupplyDetailDto
    {
        /// <summary>
        /// The asset ID to include in supply
        /// </summary>
        public long AssetId { get; set; }

        /// <summary>
        /// Optional condition note when supplying
        /// </summary>
        public string? ConditionOnSupply { get; set; }

        /// <summary>
        /// Optional custodian ID (Employee ID) for this specific asset. If not provided, uses supply-level/requester employee.
        /// </summary>
        public long? CustodianId { get; set; }

        /// <summary>
        /// Optional notes for this specific asset
        /// </summary>
        public string? Notes { get; set; }
    }
}
