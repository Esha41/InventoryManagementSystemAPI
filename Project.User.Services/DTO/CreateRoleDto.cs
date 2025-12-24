using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Role name must be between {2} and {1} characters.", MinimumLength = 2)]
        public string Name { get; set; }
        public string? NameAr { get; set; }
        public bool IsDefaultRole { get; set; } = false;
        public bool IsSuperAdmin { get; set; } = false;
        public List<int> ApplicationEntityIds { get; set; } = new List<int>();
    }
}
