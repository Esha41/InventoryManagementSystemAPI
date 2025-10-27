using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{
    public class LdapOptions
    {
        public bool IsActive { get; set; }
        public string? LdapServer { get; set; }
        public string? LdapDomain { get; set; }
        public string? LdapUsername { get; set; }
        public string? LdapPassword { get; set; }
        public string LdapEmpAttr { get; set; } = "sAMAccountName";
    }
}
