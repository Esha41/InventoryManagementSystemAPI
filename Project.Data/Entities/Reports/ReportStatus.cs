using Ettad.CrossCutting.Comman;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Reports
{
    /// <summary>
    /// Lookup table for report statuses
    /// </summary>
    public class ReportStatus : AuditEntity<long>, ILookup
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    }
}
