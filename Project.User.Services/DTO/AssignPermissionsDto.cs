using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{
    public class AssignPermissionsDto
    {
        public string EntityId { get; set; } = string.Empty;
        public List<string> PermissionsList { get; set; } = new List<string>();
    }
}
