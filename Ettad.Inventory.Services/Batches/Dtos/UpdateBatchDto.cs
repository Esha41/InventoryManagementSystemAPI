namespace Ettad.Inventory.Service.Batches.Dtos
{
    public class UpdateBatchDto
    {
        public string BatchNumber { get; set; } = string.Empty;

        public long? PrimaryPurposId { get; set; }
    }
}
