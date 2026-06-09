namespace Ettad.Data.Entities
{
    public class WeaponAccessory
    {
        public long WeaponId { get; set; }
        public long AccessoryId { get; set; }
        public long DefaultQuantity { get; set; }

        #region Navigation Properties

        public Weapon Weapon { get; set; }
        public Accessory Accessory { get; set; }

        #endregion
    }
}
