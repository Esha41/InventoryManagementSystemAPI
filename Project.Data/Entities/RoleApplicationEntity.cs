using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Idenitity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Data.Entities
{
    public class RoleApplicationEntity : FullAuditEntity<long>
    {
        public string RoleId { get; set; }
        public long ApplicationEntityId { get; set; }
    }
}
