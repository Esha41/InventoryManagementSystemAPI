namespace Ettad.Inventory.Service.AllowanceItems.Dtos
{
    public class AllowanceReserveDetailsDto
    {
        public long DepartmentId { get; set; }
        public int Year { get; set; }
        public int TotalReserve { get; set; }
        public int AvailableReserve { get; set; }
        public int OrderedQuantity { get; set; }
        public int UtilizedQuantity { get; set; }
    }
}

