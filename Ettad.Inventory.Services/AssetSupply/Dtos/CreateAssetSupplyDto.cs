namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// DTO for creating a new asset supply
    /// </summary>
    public class CreateAssetSupplyDto
    {
        /// <summary>
        /// The order ID this supply fulfills
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Optional custodian ID to assign assets to. If not provided, assets will be assigned to the order's department.
        /// </summary>
        public long? CustodianId { get; set; }

        /// <summary>
        /// Optional department ID override. If not provided, uses the order's department.
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
        /// Optional notes for this specific asset
        /// </summary>
        public string? Notes { get; set; }
    }
}

