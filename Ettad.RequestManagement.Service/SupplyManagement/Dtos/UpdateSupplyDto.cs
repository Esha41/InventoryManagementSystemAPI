namespace Ettad.RequestManagement.Service.SupplyManagement.Dtos
{
    public class UpdateSupplyDto
    {
        public string RecieverName { get; set; }
        public long ReceiverRankId { get; set; }
        public string RecieverMilitaryId { get; set; }
        public string? Notes { get; set; }
    }
}
