using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{
    public class LdapOptions
    {
        public bool IsActive { get; set; } = true;

        [Required]
        public string? LdapServer { get; set; }

        [Required]
        public string? LdapDomain { get; set; }

        [Required]
        public string? LdapUsername { get; set; }

        [Required]
        public string? LdapPassword { get; set; }

        [Required]
        public string LdapEmpAttr { get; set; } = "sAMAccountName";
    }
}
