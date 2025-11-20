namespace Ettad.RequestManagement.Service.SupplyManagement.Dtos
{
    public class SubmitSupplyDto
    {
        public string RecieverName { get; set; } = default!;
        public long ReceiverRankId { get; set; }
        public string RecieverMilitaryId { get; set; } = default!;
        public string Notes { get; set; } = default!;
    }
}

