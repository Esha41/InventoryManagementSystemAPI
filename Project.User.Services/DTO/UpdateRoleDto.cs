using System;
using System.ComponentModel.DataAnnotations;

namespace Ettad.User.Services.DTO
{
    public class UpdateRoleDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Role name must be between {2} and {1} characters.", MinimumLength = 2)]
        public string Name { get; set; }
        public bool IsDefaultRole { get; set; }
        public bool IsSuperAdmin { get; set; }
        public List<int> ApplicationEntityIds { get; set; } = new List<int>();
    }
}
