namespace Ettad.Data.Entities
{
    public class Accessory : BaseItem
    {
        #region Navigation Properties

        public ICollection<WeaponAccessory> WeaponAccessories { get; set; }

        #endregion
    }
}
