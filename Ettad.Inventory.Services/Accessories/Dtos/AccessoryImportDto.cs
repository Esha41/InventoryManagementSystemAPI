namespace Ettad.Inventory.Service.Accessories.Dtos
{
    public class AccessoryImportDto
    {
        public int RowNumber { get; set; }
        public string Name { get; set; }
        public string? NameAr { get; set; }
        public string ItemNo { get; set; }
        public string? PartNo { get; set; }
        public decimal? Price { get; set; }
        public long? MinimumQuantity { get; set; }
        public string? Nsn { get; set; }
        public string? Distribution { get; set; }
        public string? ReferenceNo { get; set; }
        public string? UNNumber { get; set; }
        public string? Notes { get; set; }
        public string? Classification { get; set; }
        public string? Type { get; set; }
    }
}
