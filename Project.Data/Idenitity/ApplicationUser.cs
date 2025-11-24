
using Ettad.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ettad.Comman.Idenitity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsLdapUser { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
        public string ExtraEmployeesView { get; set; } = string.Empty;

        public long? DepartmentId { get; set; }

        // Navigation property
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }      
        public string? FullNameEN { get; set; }
        public string? FullNameAR { get; set; }
        public long? RankId { get; set; }

        [ForeignKey(nameof(RankId))]
        public Rank Rank { get; set; }
        public string? MilitoryId { get; set; }
        public string? LdapUserName { get; set; }
        
        // Soft delete properties
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}
