namespace Ettad.Inventory.Service.Monitoring.Dtos
{
    /// <summary>Unified row for draft <see cref="Ettad.Data.Entities.Supply"/> and draft <see cref="Ettad.Data.Entities.AssetSupply"/> (monitoring list).</summary>
    public class DraftSupplyListItemDto
    {
        /// <summary>"Supply" or "AssetSupply"</summary>
        public string RowKind { get; set; } = string.Empty;

        public long RowId { get; set; }

        public long OrderId { get; set; }

        public string? OrderNumber { get; set; }

        /// <summary>Serialized <see cref="Ettad.Data.Enums.SupplySubmissionStatus"/>.</summary>
        public string SubmissionStatus { get; set; } = string.Empty;
    }
}
