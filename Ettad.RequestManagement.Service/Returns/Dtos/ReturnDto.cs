using Ettad.Module.lookup.Dtos;
using Ettad.RequestManagement.Service.Common.Dtos;

namespace Ettad.RequestManagement.Service.Returns.Dtos
{
    public class ReturnDto : BaseRequestDto
    {
        public long? ReturnToDepotId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DepotDto ReturnToDepot { get; set; }
    }
}

