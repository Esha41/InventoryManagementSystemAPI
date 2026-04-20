using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Monitoring.Dtos
{
    /// <summary>
    /// Weapon asset headline metrics for the inventory dashboard (depot-scoped).
    /// </summary>
    public class WeaponAssetDashboardDto
    {
        public int TotalAssets { get; set; }
        public int AssignedCount { get; set; }
        public int InDepotCount { get; set; }
        /// <summary>Assets with no <see cref="Asset.Status"/> set.</summary>
        public int UnknownStatusCount { get; set; }
        /// <summary>Counts per known status (excludes null status — see <see cref="UnknownStatusCount"/>).</summary>
        public List<WeaponAssetStatusCountDto> ByStatus { get; set; } = new();
    }

    public class WeaponAssetStatusCountDto
    {
        public AssetStatus Status { get; set; }
        public int Count { get; set; }
    }
}
