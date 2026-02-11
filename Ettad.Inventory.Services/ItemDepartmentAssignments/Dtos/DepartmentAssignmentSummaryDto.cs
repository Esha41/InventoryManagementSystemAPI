namespace Ettad.Inventory.Service.ItemDepartmentAssignments.Dtos
{
    /// <summary>
    /// Summary of item assignments per department with separate counts by type.
    /// </summary>
    public class DepartmentAssignmentSummaryDto
    {
        public long DepartmentId { get; set; }
        public string? DepartmentCode { get; set; } 
        public string? DepartmentNameAr { get; set; }
        public string? DepartmentNameEn { get; set; }
        public int AmmunitionCount { get; set; }
        public int ExplosivesCount { get; set; }
        public int WeaponsCount { get; set; }
    }
}
