namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// DTO for returning an asset from assignment
    /// </summary>
    public class ReturnAssetDto
    {
        /// <summary>
        /// The asset ID being returned
        /// </summary>
        public long AssetId { get; set; }

        /// <summary>
        /// The assignment ID (optional - will use current active assignment if not provided)
        /// </summary>
        public long? AssignmentId { get; set; }

        /// <summary>
        /// Condition of the asset when returned
        /// </summary>
        public string? ConditionOnReturn { get; set; }

        /// <summary>
        /// Notes about the return
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Return date (defaults to now if not provided)
        /// </summary>
        public DateTime? ReturnDate { get; set; }
    }

    /// <summary>
    /// DTO for returning multiple assets at once
    /// </summary>
    public class ReturnMultipleAssetsDto
    {
        /// <summary>
        /// List of assets to return
        /// </summary>
        public List<ReturnAssetDto> Assets { get; set; } = new();

        /// <summary>
        /// Common notes for all returns (can be overridden per asset)
        /// </summary>
        public string? CommonNotes { get; set; }
    }
}

