using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.SupplyManagement
{
	public interface ISupplyService
	{
		Task<APIOperationResponse<OrderSupplySuggestionDto>> GetSupplySuggestionAsync(long orderId);
	}
}

