using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.AllowanceItems.Dtos
{
    public class BulkAllowanceItemDto
    {
        public long ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class BulkCreateAllowanceItemDto
    {
        public long DepartmentId { get; set; }
        public int Year { get; set; }
        public List<BulkAllowanceItemDto> Items { get; set; } = new List<BulkAllowanceItemDto>();
    }
}

