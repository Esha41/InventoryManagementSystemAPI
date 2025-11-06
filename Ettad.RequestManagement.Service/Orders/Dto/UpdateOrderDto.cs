using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Orders.Dto
{
    public class UpdateOrderDto
    {
        public RequestPriority Priority { get; set; }

        #region Request Items
        public List<CreateUpdateRequestItemDto> RequestItems { get; set; }
        #endregion
    }
}

