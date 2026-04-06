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

        public int AssetCount { get; set; }

        public DepotDto Depot { get; set; }

        public PrimaryPurposDto PrimaryPurpos { get; set; }

        public List<AssetDto> Assets { get; set; } = new();
    }
}
