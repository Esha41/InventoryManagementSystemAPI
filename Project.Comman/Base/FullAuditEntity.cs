using System;
using System.Runtime.InteropServices;

namespace Ettad.CrossCutting.Comman.Base
{
    public class FullAuditEntity<T> : AuditEntity<T>, ISoftDeletable
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public string DeletedBy { get; set; }
    }
}
