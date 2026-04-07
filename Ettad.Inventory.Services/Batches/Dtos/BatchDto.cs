using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Batches.Dtos
{
    public class BatchDto
    {
        public long Id { get; set; }

        public string BatchNumber { get; set; } = string.Empty;

        public long DepotId { get; set; }

        public long? PrimaryPurposId { get; set; }

        /// <summary>Total number of assets in this batch matching the same filters (not the current page size).</summary>
        public int AssetCount { get; set; }

        /// <summary>1-based index of the current assets page when assets are paginated.</summary>
        public int AssetsPageIndex { get; set; }

        /// <summary>Page size used for the current <see cref="Assets"/> slice (when not loading all).</summary>
        public int AssetsPageSize { get; set; }

        /// <summary>Total number of pages for assets given current filters and page size.</summary>
        public int AssetsTotalPages { get; set; }

        public DepotDto Depot { get; set; }

        public PrimaryPurposDto PrimaryPurpos { get; set; }

        public List<AssetDto> Assets { get; set; } = new();
    }
}
