using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{
    public class UpdateUserRolesDto
    {
        [Required]
        public List<string> RoleNames { get; set; } = new List<string>();
    }
}
