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
        public string ExtraEmployeesView { get; set; }
        public int? EmployeeId { get; set; }
        public int? OrganizationId { get; set; }

    }

}
