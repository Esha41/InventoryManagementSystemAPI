namespace Ettad.RequestManagement.Service.Orders.Dto
{
    public class CreateUpdateRequestItemDto
    {
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public string? Notes { get; set; }

        /// <summary>
        /// For ammunition: at least one association is required (validated in <see cref="Orders.Validators.AmmunitionWeaponAssociationValidator"/>).
        /// </summary>
        public List<CreateRequestItemWeaponAssociationDto>? WeaponAssociations { get; set; }
    }
}
