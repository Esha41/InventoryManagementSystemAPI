using Ettad.Comman.Idenitity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ettad.Data.Entities
{
    public class UserDelegation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DelegatorUserId { get; set; }

        [ForeignKey("DelegatorUserId")]
        public virtual ApplicationUser DelegatorUser { get; set; }

        [Required]
        public string DelegateeUserId { get; set; }

        [ForeignKey("DelegateeUserId")]
        public virtual ApplicationUser DelegateeUser { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
