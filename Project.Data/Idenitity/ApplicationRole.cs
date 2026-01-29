using Microsoft.AspNetCore.Identity;

namespace Ettad.CrossCutting.Comman.Idenitity
{
    public class ApplicationRole : IdentityRole
    {
        public string? NameAr { get; set; }
        public bool? IsDefaultRole { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsAdmin { get; set; }
    }
}
