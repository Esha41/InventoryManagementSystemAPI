using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Inventories;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.Extensions.Logging;

namespace Ettad.RequestManagement.Service.SupplyManagement
{
    public class SupplyService : ISupplyService
    {
        private readonly IInventoryService _inventoryService;
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<SupplyService> _logger;

        public SupplyService(
            IInventoryService inventoryService,
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<InventoryDetail> inventoryDetailRepository,
            ICurrentUserService currentUserService,
            ILogger<SupplyService> logger)
        {
            _inventoryService = inventoryService;
            _orderRepository = orderRepository;
            _inventoryDetailRepository = inventoryDetailRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<OrderSupplySuggestionDto>> GetSupplySuggestionAsync(long orderId)
        {
            _logger.LogInformation("Getting supply suggestion for order. OrderId: {OrderId}, User: {UserId}", 
                orderId, _currentUserService.UserId);

            try
            {
                // Delegate to InventoryService for the actual suggestion logic
                var result = await _inventoryService.SuggestSupplyForOrderAsync(orderId);

                if (result.Success)
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

        public async Task<APIOperationResponse<long>> CreateSupplyAsync(CreateSupplyDto inputDto)
        {
            _logger.LogInformation("Creating supply transaction. OrderId: {OrderId}, ItemCount: {ItemCount}, User: {UserId}", 
                inputDto.OrderId, inputDto.SupplyItems?.Count ?? 0, _currentUserService.UserId);

            try
            {
                // Validate order exists
                var order = await _orderRepository.FindOneAsync(o => o.Id == inputDto.OrderId && !o.IsDeleted);
                if (order == null)
                {
                    _logger.LogWarning("Order not found for supply. OrderId: {OrderId}, User: {UserId}", 
                        inputDto.OrderId, _currentUserService.UserId);
                    return APIOperationResponse<long>.Fail(ResponseType.NotFound, "Order not found");
                }

                if (inputDto.SupplyItems == null || !inputDto.SupplyItems.Any())
                {
                    _logger.LogWarning("No supply items provided. OrderId: {OrderId}, User: {UserId}", 
                        inputDto.OrderId, _currentUserService.UserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Supply items are required");
                }

                // Process each supply item and update inventory quantities
                foreach (var supplyItem in inputDto.SupplyItems)
                {
                    var inventoryDetail = await _inventoryDetailRepository.FindOneAsync(
                        id => id.Id == supplyItem.InventoryDetailId);

                    if (inventoryDetail == null)
                    {
                        _logger.LogWarning("Inventory detail not found. InventoryDetailId: {InventoryDetailId}, OrderId: {OrderId}", 
                            supplyItem.InventoryDetailId, inputDto.OrderId);
                        return APIOperationResponse<long>.Fail(
                            ResponseType.NotFound, 
                            $"Inventory detail {supplyItem.InventoryDetailId} not found");
                    }

                    if (inventoryDetail.ItemQuantity < supplyItem.Quantity)
                    {
                        _logger.LogWarning("Insufficient inventory quantity. InventoryDetailId: {InventoryDetailId}, Available: {Available}, Requested: {Requested}", 
                            supplyItem.InventoryDetailId, inventoryDetail.ItemQuantity, supplyItem.Quantity);
                        return APIOperationResponse<long>.Fail(
                            ResponseType.BadRequest, 
                            $"Insufficient quantity in inventory detail {supplyItem.InventoryDetailId}. Available: {inventoryDetail.ItemQuantity}, Requested: {supplyItem.Quantity}");
                    }

                    // Deduct the quantity from inventory
                    inventoryDetail.ItemQuantity -= supplyItem.Quantity;
                    await _inventoryDetailRepository.UpdateAsync(inventoryDetail);

                    _logger.LogInformation("Inventory updated. InventoryDetailId: {InventoryDetailId}, QuantitySupplied: {Quantity}, RemainingQuantity: {Remaining}", 
                        supplyItem.InventoryDetailId, supplyItem.Quantity, inventoryDetail.ItemQuantity);
                }

                // Update order status (you may want to add a status like "Supplied" or "Fulfilled")
                // This depends on your business logic and enum values
                // order.Status = RequestStatus.Fulfilled;
                // await _orderRepository.UpdateAsync(order);

                _logger.LogInformation("Supply transaction completed successfully. OrderId: {OrderId}, ItemsSupplied: {ItemCount}, User: {UserId}", 
                    inputDto.OrderId, inputDto.SupplyItems.Count, _currentUserService.UserId);

                // Return the order ID as the supply transaction ID
                // In a real scenario, you might want to create a separate Supply entity to track this
                return APIOperationResponse<long>.Success(inputDto.OrderId, "Supply transaction completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating supply transaction. OrderId: {OrderId}, User: {UserId}", 
                    inputDto.OrderId, _currentUserService.UserId);
                return APIOperationResponse<long>.Fail(
                    ResponseType.InternalServerError, 
                    $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<SupplyDto>> GetSupplyByOrderIdAsync(long orderId)
        {
            _logger.LogInformation("Getting supply history for order. OrderId: {OrderId}, User: {UserId}", 
                orderId, _currentUserService.UserId);

            try
            {
                // Validate order exists
                var order = await _orderRepository.FindOneAsync(
                    o => o.Id == orderId && !o.IsDeleted,
                    false,
                    nameof(Order.RequestItems));

                if (order == null)
                {
                    _logger.LogWarning("Order not found. OrderId: {OrderId}, User: {UserId}", 
                        orderId, _currentUserService.UserId);
                    return APIOperationResponse<SupplyDto>.Fail(ResponseType.NotFound, "Order not found");
                }

                // Note: This is a placeholder implementation
                // In a real scenario, you would query a Supply entity table to get the actual supply history
                // For now, we return a basic DTO with order information
                var supplyDto = new SupplyDto
                {
                    Id = orderId,
                    OrderId = orderId,
                    OrderNo = order.RequestNo,
                    SupplyDate = DateTime.UtcNow,
                    SuppliedBy = _currentUserService.UserId ?? "System",
                    SupplyItems = new List<SupplyItemDto>()
                };

                _logger.LogInformation("Supply history retrieved. OrderId: {OrderId}", orderId);
                
                return APIOperationResponse<SupplyDto>.Success(
                    supplyDto, 
                    "Note: Full supply history tracking requires Supply entity implementation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting supply history. OrderId: {OrderId}, User: {UserId}", 
                    orderId, _currentUserService.UserId);
                return APIOperationResponse<SupplyDto>.Fail(
                    ResponseType.InternalServerError, 
                    $"An error occurred: {ex.Message}");
            }
        }
    }
}

