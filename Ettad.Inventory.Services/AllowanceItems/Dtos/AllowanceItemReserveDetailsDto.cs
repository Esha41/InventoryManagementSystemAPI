namespace Ettad.Inventory.Service.AllowanceItems.Dtos
{
    public class AllowanceItemReserveDetailsDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string? ItemNameAr { get; set; }
        public string ItemNo { get; set; }
        public string? BatchNo { get; set; }
        public int OriginalQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public int ReservedQuantityByOrdersOnProcessing { get; set; }
        public int UsedQuantity { get; set; }
    }

    public class AllowanceReserveDetailsByItemDto
    {
        public long DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentNameAr { get; set; }
        public string DepartmentNameEn { get; set; }
        public int Year { get; set; }
        public int TotalOriginalQuantity { get; set; }
        public int TotalRemainingQuantity { get; set; }
        public int TotalReservedQuantityByOrdersOnProcessing { get; set; }
        public int TotalUsedQuantity { get; set; }
        public List<AllowanceItemReserveDetailsDto> Items { get; set; } = new List<AllowanceItemReserveDetailsDto>();
    }
}

