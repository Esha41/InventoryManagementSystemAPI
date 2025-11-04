using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.AllowanceItems.Dtos
{
    public class CreateUpdateAllowanceItemDto
    {
        public long ItemId { get; set; }
        public long DepartmentId { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }
}

