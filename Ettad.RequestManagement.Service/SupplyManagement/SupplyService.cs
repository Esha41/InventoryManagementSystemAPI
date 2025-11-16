using Ettad.Application.Common.Interfaces;
using Ettad.Inventory.Service.Inventories;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.Extensions.Logging;

namespace Ettad.RequestManagement.Service.SupplyManagement
{
	public class SupplyService : ISupplyService
	{
		private readonly IInventoryService _inventoryService;
		private readonly ICurrentUserService _currentUserService;
		private readonly ILogger<SupplyService> _logger;

		public SupplyService(
			IInventoryService inventoryService,
			ICurrentUserService currentUserService,
			ILogger<SupplyService> logger)
		{
			_inventoryService = inventoryService;
			_currentUserService = currentUserService;
			_logger = logger;
		}

		public async Task<APIOperationResponse<OrderSupplySuggestionDto>> GetSupplySuggestionAsync(long orderId)
		{
			_logger.LogInformation("Getting supply suggestion for order. OrderId: {OrderId}, User: {UserId}", 
				orderId, _currentUserService.UserId);

			try
			{
				var result = await _inventoryService.SuggestSupplyForOrderAsync(orderId);

				if (result.Succeeded)
				{
					_logger.LogInformation("Supply suggestion retrieved successfully. OrderId: {OrderId}, CanFulfill: {CanFulfill}", 
						orderId, result.Data?.CanFulfillCompletely);
				}
				else
				{
					_logger.LogWarning("Failed to get supply suggestion. OrderId: {OrderId}, Error: {Error}", 
						orderId, result.Message);
				}

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting supply suggestion. OrderId: {OrderId}, User: {UserId}", 
					orderId, _currentUserService.UserId);
				return APIOperationResponse<OrderSupplySuggestionDto>.Fail(
					ResponseType.InternalServerError, 
					$"An error occurred: {ex.Message}");
			}
		}
	}
}

