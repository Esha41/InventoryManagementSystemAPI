using Moujam.Casiher.Comman.Base;
using System.ComponentModel.DataAnnotations;

namespace BrzanData.Models
{
    public abstract class BaseItem : AuditEntity<long> //Base  for Ammunation , Explosive , Weapon , Accessory
    {
        [Required, MaxLength(200)]
        public int ItemNo { get; set; }

        [Required, MaxLength(200)]
        public int Lot { get; set; }
        
        [Required, MaxLength(200)]
        public string BatchNo { get; set; }
        
        [Required, MaxLength(200)]
        public int HccId  { get; set; }
        
        [Required, MaxLength(200)]
        public int Manufacture { get; set; }
        
        [Required, MaxLength(200)]
        public int SupplierId { get; set; }
        
        [Required, MaxLength(200)]
        public int CountryId { get; set; }
        
        [Required, MaxLength(200)]
        public int Quantity { get; set; }

        [Required, MaxLength(200)]
        public int PartNo { get; set; }

        [Required, MaxLength(200)]
        public required string Depot { get; set; }
    }
}
