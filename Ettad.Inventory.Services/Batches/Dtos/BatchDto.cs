using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Batches.Dtos
{
    public class BatchDto
    {
        public long Id { get; set; }

        public string BatchNumber { get; set; } = string.Empty;

        public long DepotId { get; set; }

        /// <summary>Total number of assets in this batch matching the same filters (not the current page size).</summary>
        public int AssetCount { get; set; }

        /// <summary>1-based index of the current assets page when assets are paginated.</summary>
        public int AssetsPageIndex { get; set; }

        /// <summary>Page size used for the current <see cref="Assets"/> slice (when not loading all).</summary>
        public int AssetsPageSize { get; set; }

        /// <summary>Total number of pages for assets given current filters and page size.</summary>
        public int AssetsTotalPages { get; set; }

        /// <summary>
        /// Asset counts grouped by catalog item for the batch (same filters as the batch asset list).
        /// Empty when there are no matching assets.
        /// </summary>
        public List<BatchAssetItemCountDto> AssetItemCounts { get; set; } = new();

        public DepotDto Depot { get; set; }

        public List<AssetDto> Assets { get; set; } = new();
    }
}
