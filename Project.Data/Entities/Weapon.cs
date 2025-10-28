using System.ComponentModel.DataAnnotations;

namespace BrzanData.Models
{
    public class Weapon :BaseItem
    {
        [Required, MaxLength(200)]
        public required string Status { get; set; }
    }
}
