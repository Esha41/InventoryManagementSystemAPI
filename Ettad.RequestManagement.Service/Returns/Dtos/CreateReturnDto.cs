namespace Ettad.RequestManagement.Service.Returns.Dtos
{
    public class CreateReturnDto
    {
        public string? Reason { get; set; }
        public string Priority { get; set; }
        public string? Notes { get; set; }
        public long DepartmentId { get; set; }
        public long? RequesterId { get; set; }
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

