using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.CrossCutting.Comman.Base
{
    public class AuditEntity<T> : BaseEntity<T>
    {
        public DateTime CreationDate { get; set; }

        public DateTime? ModificationDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
    }
    public class AuditEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
    }
}
