using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public bool IsDefaultRole { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsAdmin { get; set; }
        public List<long> ApplicationEntityIds { get; set; } = new List<long>();
    }
}
