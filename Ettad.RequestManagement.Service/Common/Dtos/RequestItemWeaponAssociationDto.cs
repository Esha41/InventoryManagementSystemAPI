namespace Ettad.RequestManagement.Service.Common.Dtos
{
    public class RequestItemWeaponAssociationDto
    {
        public long Id { get; set; }

        public long? AssociatedWeaponItemId { get; set; }

        public string? AssociatedWeaponOtherName { get; set; }

        public long? AssociatedWeaponCaliberId { get; set; }

        public string? AssociatedWeaponName { get; set; }
    }
}
