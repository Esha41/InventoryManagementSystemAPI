namespace Ettad.Inventory.Service.Assets.Dtos
{
    public class EmployeeExcelImportRowDto
    {
        public int RowNumber { get; set; }

        /// <summary>Hidden round-trip column; when > 0, row updates the existing employee.</summary>
        public long EmployeeId { get; set; }

        public string? NameAr { get; set; }

        public string? NameEn { get; set; }

        public string? MilitaryId { get; set; }

        /// <summary>Department display from Excel dropdown (resolved to <see cref="DepartmentId"/>).</summary>
        public string? DepartmentName { get; set; }

        /// <summary>Resolved during enrichment from <see cref="DepartmentName"/>.</summary>
        public long? DepartmentId { get; set; }

        /// <summary>Rank display from Excel dropdown (resolved to <see cref="RankId"/>).</summary>
        public string? RankName { get; set; }

        /// <summary>Resolved during enrichment from <see cref="RankName"/>.</summary>
        public long? RankId { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Notes { get; set; }
    }
}
