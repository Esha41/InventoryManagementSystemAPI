using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrzanData.Models
{
    public class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        // Navigation properties for joins
        public Weapon? Weapon { get; set; }
        public Ammunition? Ammunition { get; set; }
        public Explosive? Explosive { get; set; }
        public Accessory? Accessory { get; set; }

        [Required]
        public int DepoId { get; set; }
        public Depo? Depo { get; set; }

        [Required]
        public int ItemQuantity { get; set; }

        [Required]
        public int CurrentQuantity { get; set; }

       


        [Required]
        public int MinimumStockLevel { get; set; }

        [Required]
        public int MaximumStockLevel { get; set; }
                   
        [Required]
        public DateTime CreatedDate { get; set; }
       
        [MaxLength(500)]
        public string? Notes { get; set; }

       
    }
}
