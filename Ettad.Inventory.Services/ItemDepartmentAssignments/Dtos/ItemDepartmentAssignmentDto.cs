using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.ItemDepartmentAssignments.Dtos
{
    public class ItemDepartmentAssignmentDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public ItemType ItemType { get; set; }
        public long DepartmentId { get; set; }
        public string? DepartmentCode { get; set; }
        public string? DepartmentNameAr { get; set; }
        public string? DepartmentNameEn { get; set; }
        public string? Notes { get; set; }
    }
}
