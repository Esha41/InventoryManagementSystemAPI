using Ettad.RequestManagement.Service.Common.Dtos;

namespace Ettad.RequestManagement.Service.Orders.Dto
{
    /// <summary>
    /// Order-specific request item DTO.
    /// Currently inherits all properties from RequestItemDto.
    /// Kept as separate class for potential future Order-specific item properties.
    /// </summary>
    public class OrderRequestItemDto : RequestItemDto
    {
        // All properties inherited from RequestItemDto (including ItemType)
    }
}

