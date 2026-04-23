using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO.Organization
{
    public class OrganizationCreateModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
