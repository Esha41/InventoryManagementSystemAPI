using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Returns.Dtos
{
    public class CreateReturnDto
    {
        public string? Reason { get; set; }
        public RequestPriority Priority { get; set; }
        public string? Notes { get; set; }
        public string RequestPurposeNotes { get; set; } = string.Empty;
        public long RequestPurposeId { get; set; }
        public List<CreateReturnItemDto> ReturnItems { get; set; } = new();
    }

    public class CreateReturnItemDto
    {
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public string? Notes { get; set; }
    }
}

