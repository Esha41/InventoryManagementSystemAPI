namespace Ettad.Inventory.Service.Inventories.Dtos
{
    /// <summary>
    /// Groups lot suggestions for a single order item
    /// </summary>
    public class OrderItemSupplySuggestionDto
    {
        public long RequestItemId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public long RequestedQuantity { get; set; }
        public long SuggestedQuantity { get; set; }
        public bool CanFulfillCompletely { get; set; }
        public List<SupplyLotSuggestionDto> LotSuggestions { get; set; } = new List<SupplyLotSuggestionDto>();
    }
}

