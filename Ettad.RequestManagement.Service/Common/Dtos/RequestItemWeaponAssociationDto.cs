namespace Ettad.RequestManagement.Service.Common.Dtos
{
    public class RequestItemWeaponAssociationDto
    {
        public long Id { get; set; }

        public long? AssociatedWeaponItemId { get; set; }

        public string? AssociatedWeaponOtherName { get; set; }

        public long? AssociatedWeaponCaliberId { get; set; }

        public string? AssociatedWeaponName { get; set; }

        public string? AssociatedWeaponNameAr { get; set; }

        /// <summary>Catalog weapon caliber (for display / compatibility on read models).</summary>
        public long? AssociatedWeaponCatalogCaliberId { get; set; }

        public string? AssociatedWeaponCatalogCaliberNameEn { get; set; }

        public string? AssociatedWeaponCatalogCaliberNameAr { get; set; }
    }
}
