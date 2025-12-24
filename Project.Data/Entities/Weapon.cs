namespace Ettad.Data.Entities
{
    public class Weapon : BaseItem
    {
        public string? Caliber { get; set; }
        public long? CaliberUnitId { get; set; }
        public int? YearOfManufacture { get; set; }
        public long? CountryOfManufactureId { get; set; }
        public string? Model { get; set; }

        #region Navigation Properties
        public Unit CaliberUnit { get; set; }
        public Country CountryOfManufacture { get; set; }
        #endregion
    }
}
