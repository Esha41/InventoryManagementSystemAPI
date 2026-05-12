using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class RequestItem : FullAuditEntity<long>
    {
        public long ItemId { get; set; }

        public long Quantity { get; set; }

        public long RequestId { get; set; }

        public string Notes { get; set; }

        /// <summary>
        /// Ammunition lines: one or more intended weapons (catalog id and/or custom name per row).
        /// Empty for non-ammunition items.
        /// </summary>
        public ICollection<RequestItemWeaponAssociation> WeaponAssociations { get; set; } =
            new List<RequestItemWeaponAssociation>();

        #region Navigation Properties

        public BaseItem Item { get; set; }
        public BaseRequest Request { get; set; }

        #endregion
    }
}
