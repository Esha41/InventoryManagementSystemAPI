using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// One row per weapon intended for use with an ammunition request line (request item).
    /// Multiple rows per <see cref="RequestItem"/> are allowed when ammunition applies to several weapons.
    /// </summary>
    public class RequestItemWeaponAssociation : FullAuditEntity<long>
    {
        public long RequestItemId { get; set; }

        public RequestItem RequestItem { get; set; }

        /// <summary>Catalog weapon item id when <see cref="AssociatedWeaponOtherName"/> is null.</summary>
        public long? AssociatedWeaponItemId { get; set; }

        /// <summary>Custom weapon name when not in catalog; mutually exclusive with catalog id per row.</summary>
        public string? AssociatedWeaponOtherName { get; set; }

        /// <summary>Ammunition caliber side FK used for compatibility validation (matches ammunition row).</summary>
        public long? AssociatedWeaponCaliberId { get; set; }
    }
}
