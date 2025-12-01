namespace Ettad.Inventory.Service.AllowanceItems.Dtos
{
    public class AllowanceReserveDetailsDto
    {
        public long DepartmentId { get; set; }
        public int Year { get; set; }
        public int TotalOriginalQuantity { get; set; }
        public int TotalRemainingQuantity { get; set; }
        public int TotalReservedQuantityByOrdersOnProcessing { get; set; }
        public int TotalUsedQuantity { get; set; }
    }
}

