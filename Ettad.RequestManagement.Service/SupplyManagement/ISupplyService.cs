using Ettad.Data.Enums;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.SupplyManagement
{
	public interface ISupplyService
	{
		Task<APIOperationResponse<OrderSupplySuggestionDto>> GetSupplySuggestionAsync(long orderId, List<long>? depotIds = null);
		Task<APIOperationResponse<SupplyDto>> GetByIdAsync(long id);
		Task<APIOperationResponse<List<SupplyDto>>> GetAllAsync();
		Task<APIOperationResponse<long>> CreateAsync(CreateSupplyDto inputDto);
		Task<APIOperationResponse<bool>> UpdateSupplyInfoAsync(long id, UpdateSupplyDto inputDto);
		Task<APIOperationResponse<long>> AddSupplyDetailAsync(long supplyId, CreateSupplyDetailDto detailDto);
		Task<APIOperationResponse<bool>> UpdateSupplyDetailAsync(long supplyId, long detailId, UpdateSupplyDetailDto detailDto);
		Task<APIOperationResponse<bool>> DeleteSupplyDetailAsync(long supplyId, long detailId);
		Task<APIOperationResponse<bool>> UpdateSubmissionStatusAsync(long id, SupplySubmissionStatus newStatus);
	}
}

