namespace Ettad.Inventory.Service.Monitoring.Dtos
{
    /// <summary>Approved order that is not fully fulfilled per monitoring rules.</summary>
    public class OrderAwaitingFulfillmentListItemDto
    {
        public long OrderId { get; set; }

        public string? OrderNumber { get; set; }

        /// <summary>Serialized <see cref="Ettad.Data.Enums.RequestStatus"/>.</summary>
        public string Status { get; set; } = string.Empty;
    }
}
