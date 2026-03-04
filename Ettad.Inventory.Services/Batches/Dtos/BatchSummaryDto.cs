namespace Ettad.Inventory.Service.Batches.Dtos
{
    /// <summary>
    /// Lightweight DTO for batch list - id, batch number, and quantity only.
    /// </summary>
    public class BatchSummaryDto
    {
        public long Id { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
