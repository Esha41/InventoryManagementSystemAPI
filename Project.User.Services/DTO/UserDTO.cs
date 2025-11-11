using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{

    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsLdapUser { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string ExtraEmployeesView { get; set; }
        public long? DeparmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? OrganizationId { get; set; }
        public List<string> RoleIds { get; set; } = new();
        public string? FullNameEN { get; set; }
        public string? FullNameAR { get; set; }
        public long? RankId { get; set; }
        public string? MilitoryId { get; set; }

    }

}
