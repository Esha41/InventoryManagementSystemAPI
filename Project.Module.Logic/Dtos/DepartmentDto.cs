using Ettad.Data.Entities;

namespace Ettad.Module.lookup.Dtos
{
    public class DepartmentDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
