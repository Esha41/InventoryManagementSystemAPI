using Ettad.CrossCutting.Comman;
using Ettad.CrossCutting.Comman.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Data.Entities
{
    public class WorkFlowType : AuditEntity<long>, ILookup
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
