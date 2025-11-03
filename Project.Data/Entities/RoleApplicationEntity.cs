using Ettad.CrossCutting.Comman.Idenitity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Data.Entities
{
    public class RoleApplicationEntity
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public int ApplicationEntityId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }

        
    }
}
