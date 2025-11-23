using Ettad.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.CrossCutting.Comman.Idenitity
{
    public class ApplicationRole : IdentityRole
    {
        public string? NameAr { get; set; }
        public bool? IsDefaultRole { get; set; }
        public bool IsSuperAdmin { get; set; }
        public long? ApplicationEntityId { get; set; }

        [ForeignKey(nameof(ApplicationEntityId))]
        public ApplicationEntity ApplicationEntity { get; set; }
    }
}
