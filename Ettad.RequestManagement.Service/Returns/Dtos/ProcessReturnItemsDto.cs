using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Returns.Dtos
{
    public class ProcessReturnItemsDto
    {
        public List<ReturnAmmoExplosiveItemDto> AmmoExplosiveItems { get; set; } = new();
        public List<ReturnWeaponItemDto> WeaponItems { get; set; } = new();

        /// <summary>Optional completion notes (stored as the current approval step comments).</summary>
        public string? WorkflowStepComments { get; set; }
    }

    public class ReturnAmmoExplosiveItemDto
    {
        public long ItemId { get; set; }

        public long Quantity { get; set; }

        /// <summary>Snapshot of quantity from the return request line (audit). When omitted, <see cref="Quantity"/> is used.</summary>
        public long? ReturnedQuantity { get; set; }

        public string Lot { get; set; }

        public string? Notes { get; set; }

        public long? RequestItemId { get; set; }

        /// <summary>Whether returned stock is ready for issue; persisted on <c>InventoryDetail.ReadyForIssue</c>.</summary>
        public bool ReadyForIssue { get; set; }
    }

    public class ReturnWeaponItemDto
    {
        public long ItemId { get; set; }

        public string SerialNumber { get; set; }

        public string BatchNumber { get; set; }

        /// <summary>Asset condition after receipt; persisted on <c>Asset.Status</c>.</summary>
        public AssetStatus Status { get; set; }

        /// <summary>Row notes; persisted on <c>Asset.Notes</c> (replace on this action).</summary>
        public string? Notes { get; set; }

        public long? RequestItemId { get; set; }
    }
}
