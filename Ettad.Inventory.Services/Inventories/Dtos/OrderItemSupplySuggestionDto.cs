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

        /// <summary>Available remaining stock (total − hold − supplied) across selected depots.</summary>
        public long RemainingQuantity { get; set; }

        /// <summary>Low-stock threshold from catalog; null when not configured.</summary>
        public long? MinimumQuantity { get; set; }

        /// <summary>Critical-stock threshold from catalog; null when not configured.</summary>
        public long? CriticalQuantity { get; set; }

        /// <summary>Draft supply quantity already reserved for this order item.</summary>
        public long DraftHoldQuantity { get; set; }
    }
}

