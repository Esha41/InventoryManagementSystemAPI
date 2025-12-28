using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Assets.Dtos
{
    public class EmployeeDto
    {
        public long Id { get; set; }

        public string? NameAr { get; set; }

        public string? NameEn { get; set; }

        public string? MilitaryId { get; set; }

        public long? DepartmentId { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Notes { get; set; }

        public bool IsDeleted { get; set; }

        #region Navigation Properties

        public DepartmentDto Department { get; set; }

        #endregion
    }
}

