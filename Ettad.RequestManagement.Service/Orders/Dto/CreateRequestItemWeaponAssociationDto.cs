namespace Ettad.RequestManagement.Service.Orders.Dto
{
    public class CreateRequestItemWeaponAssociationDto
    {
        public long? AssociatedWeaponItemId { get; set; }

        public string? AssociatedWeaponOtherName { get; set; }

        public long? AssociatedWeaponCaliberId { get; set; }
    }
}
