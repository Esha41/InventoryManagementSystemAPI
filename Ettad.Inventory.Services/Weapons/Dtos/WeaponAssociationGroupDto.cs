namespace Ettad.Inventory.Service.Weapons.Dtos
{
    /// <summary>
    /// Weapons allowed for catalog association for one ammunition caliber id (issue request wizard).
    /// </summary>
    public class WeaponAssociationGroupDto
    {
        public long AmmunitionCaliberId { get; set; }
        public List<WeaponDto> Weapons { get; set; } = new();
    }
}
