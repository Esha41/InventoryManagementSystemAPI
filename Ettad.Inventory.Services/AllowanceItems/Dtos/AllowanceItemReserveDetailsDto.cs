namespace Ettad.Inventory.Service.AllowanceItems.Dtos
{
    public class AllowanceItemReserveDetailsDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        public string BatchNo { get; set; }
        public int TotalReserve { get; set; }
        public int AvailableReserve { get; set; }
        public int OrderedQuantity { get; set; }
        public int UtilizedQuantity { get; set; }
    }

    public class AllowanceReserveDetailsByItemDto
    {
        public long DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentNameAr { get; set; }
        public string DepartmentNameEn { get; set; }
        public int Year { get; set; }
        public int TotalReserve { get; set; }
        public int TotalAvailableReserve { get; set; }
        public int TotalOrderedQuantity { get; set; }
        public int TotalUtilizedQuantity { get; set; }
        public List<AllowanceItemReserveDetailsDto> Items { get; set; } = new List<AllowanceItemReserveDetailsDto>();
    }
}

