namespace Ettad.Inventory.Service.ItemDepartmentAssignments.Dtos
{
    public class CreateUpdateItemDepartmentAssignmentDto
    {
        public long ItemId { get; set; }
        public long DepartmentId { get; set; }
        public string? Notes { get; set; }
    }
}
