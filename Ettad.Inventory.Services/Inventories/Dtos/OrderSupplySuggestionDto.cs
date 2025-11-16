namespace Ettad.Inventory.Service.Inventories.Dtos
{
    /// <summary>
    /// Complete supply plan for an order with all item suggestions
    /// </summary>
    public class OrderSupplySuggestionDto
    {
        public long OrderId { get; set; }
        public string OrderNo { get; set; }
        public long DepartmentId { get; set; }
        public bool CanFulfillCompletely { get; set; }
        public List<OrderItemSupplySuggestionDto> ItemSuggestions { get; set; } = new List<OrderItemSupplySuggestionDto>();
        public string Message { get; set; }
    }
}

