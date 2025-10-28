using System.ComponentModel.DataAnnotations;

namespace BrzanData.Models
{
    public class InventoryDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int DepoId { get; set; }
        public int ItemQuantity { get; set; }
        public int CurrentQuantity { get; set; }
        public int MinimumStockLevel { get; set; }
        public int MaximumStockLevel { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Notes { get; set; }

        // Joined data
        public DepoDto? Depo { get; set; }
        public WeaponDto? Weapon { get; set; }
        public AmmunitionDto? Ammunition { get; set; }
        public ExplosiveDto? Explosive { get; set; }
        public AccessoryDto? Accessory { get; set; }
    }

    public class DepoDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class WeaponDto
    {
        public int Id { get; set; }
        public int ItemNo { get; set; }
        public int Lot { get; set; }
        public string BatchNo { get; set; } = string.Empty;
        public int Hcc { get; set; }
        public int Manufacture { get; set; }
        public int Supplier { get; set; }
        public int Country { get; set; }
        public int Quantity { get; set; }
        public int PartNo { get; set; }
        public string Depot { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class AmmunitionDto
    {
        public int Id { get; set; }
        public int ItemNo { get; set; }
        public int Lot { get; set; }
        public string BatchNo { get; set; } = string.Empty;
        public int Hcc { get; set; }
        public int Manufacture { get; set; }
        public int Supplier { get; set; }
        public int Country { get; set; }
        public int Quantity { get; set; }
        public int PartNo { get; set; }
        public string Depot { get; set; } = string.Empty;
        public int BulletDiameters { get; set; }
        public int CaseLength { get; set; }
        public bool IsLinked { get; set; }
        public int NatureOption { get; set; }
        public int Ncc { get; set; }
        public int Ncn { get; set; }
        public int PrimaryPurpos { get; set; }
        public int ProjectileColor { get; set; }
        public int TotalWeight { get; set; }
        public int ProjectileMaterial { get; set; }
        public int CaseType { get; set; }
        public int Primer { get; set; }
        public int Probelant { get; set; }
        public int HazardDivsion { get; set; }
        public int CompabilityGroup { get; set; }
    }

    public class ExplosiveDto
    {
        public int Id { get; set; }
        public int ItemNo { get; set; }
        public int Lot { get; set; }
        public string BatchNo { get; set; } = string.Empty;
        public int Hcc { get; set; }
        public int Manufacture { get; set; }
        public int Supplier { get; set; }
        public int Country { get; set; }
        public int Quantity { get; set; }
        public int PartNo { get; set; }
        public string Depot { get; set; } = string.Empty;
    }

    public class AccessoryDto
    {
        public int Id { get; set; }
        public int ItemNo { get; set; }
        public int Lot { get; set; }
        public string BatchNo { get; set; } = string.Empty;
        public int Hcc { get; set; }
        public int Manufacture { get; set; }
        public int Supplier { get; set; }
        public int Country { get; set; }
        public int Quantity { get; set; }
        public int PartNo { get; set; }
        public string Depot { get; set; } = string.Empty;
    }
}
