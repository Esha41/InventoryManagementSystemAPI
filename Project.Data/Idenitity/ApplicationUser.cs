
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

        public int? EmployeeId { get; set; }
        //  public Employee Employee { get; set; }

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
    }
}
