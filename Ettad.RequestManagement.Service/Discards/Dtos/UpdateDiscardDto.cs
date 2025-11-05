namespace Ettad.RequestManagement.Service.Discards.Dtos
{
    public class UpdateDiscardDto
    {
        public string Reason { get; set; }
        public string Priority { get; set; }
        public string? Notes { get; set; }
        public long DepartmentId { get; set; }
        public long? RequesterId { get; set; }
        public long? RecieverId { get; set; }
        public long? DepotId { get; set; }
        public long RequestPurposeId { get; set; }
        public List<UpdateDiscardItemDto> DiscardItems { get; set; } = new();
    }

    public class UpdateDiscardItemDto
    {
        public long? Id { get; set; } // Nullable for new items being added during update
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public string? Notes { get; set; }
    }
}

