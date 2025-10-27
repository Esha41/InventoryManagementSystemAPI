using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.CrossCutting.Comman.Idenitity
{
    public class ApplicationRole : IdentityRole
    {
        public bool? IsDefaultRole { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}
