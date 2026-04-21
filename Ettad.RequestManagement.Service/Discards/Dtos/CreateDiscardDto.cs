using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Discards.Dtos
{
    public class CreateDiscardDto
    {
        public string? Reason { get; set; }
        public RequestPriority Priority { get; set; }
        public string? Notes { get; set; }
        public string RequestPurposeNotes { get; set; } = string.Empty;
        public long RequestPurposeId { get; set; }
        public List<CreateDiscardItemDto> DiscardItems { get; set; } = new();
    }

    public class CreateDiscardItemDto
    {
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
