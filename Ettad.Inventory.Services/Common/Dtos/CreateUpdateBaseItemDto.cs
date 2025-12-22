namespace Ettad.Inventory.Service.Common.Dtos
{
    public class CreateUpdateBaseItemDto
    {
        public string Name { get; set; }

        public string ItemNo { get; set; }

        // All other fields are optional - only Name and ItemNo are required

        public string? PartNo { get; set; }

        public decimal? Price { get; set; }

        public long? MinimumQuantity { get; set; }

        public string? Nsn { get; set; }

        public string? Distribution { get; set; }

        public string? ReferenceNo { get; set; }

        public string? UNNumber { get; set; }

        public string? Notes { get; set; }

        public long? ClassificationId { get; set; }

        public long? TypeId { get; set; }
    }
}

