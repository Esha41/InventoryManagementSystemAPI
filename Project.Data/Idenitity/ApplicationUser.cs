
using Microsoft.AspNetCore.Identity;

namespace Ettad.Comman.Idenitity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsLdapUser { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
        public string ExtraEmployeesView { get; set; } = string.Empty;

        public int? EmployeeId { get; set; }
      //  public Employee Employee { get; set; }

        public int? OrganizationId { get; set; }
      //  public Organization Organization { get; set; }
    }
}
