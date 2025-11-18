using AutoMapper;
using FluentValidation;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
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
		private readonly ICrossCuttingRepository<Supply> _supplyRepository;
		private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailRepository;
		private readonly ICrossCuttingRepository<Order> _orderRepository;
		private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
		private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailRepository;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
		private readonly IValidator<CreateSupplyDto> _createValidator;
		private readonly IValidator<UpdateSupplyDto> _updateValidator;
		private readonly IValidator<CreateSupplyDetailDto> _createDetailValidator;
		private readonly IValidator<UpdateSupplyDetailDto> _updateDetailValidator;
		private readonly ILogger<SupplyService> _logger;

		public SupplyService(
			IInventoryService inventoryService,
			ICrossCuttingRepository<Supply> supplyRepository,
			ICrossCuttingRepository<SupplyDetail> supplyDetailRepository,
			ICrossCuttingRepository<Order> orderRepository,
			ICrossCuttingRepository<RequestItem> requestItemRepository,
			ICrossCuttingRepository<InventoryDetail> inventoryDetailRepository,
			ICurrentUserService currentUserService,
			IMapper mapper,
			IValidator<CreateSupplyDto> createValidator,
			IValidator<UpdateSupplyDto> updateValidator,
			IValidator<CreateSupplyDetailDto> createDetailValidator,
			IValidator<UpdateSupplyDetailDto> updateDetailValidator,
			ILogger<SupplyService> logger)
		{
			_inventoryService = inventoryService;
			_supplyRepository = supplyRepository;
			_supplyDetailRepository = supplyDetailRepository;
			_orderRepository = orderRepository;
			_requestItemRepository = requestItemRepository;
			_inventoryDetailRepository = inventoryDetailRepository;
			_currentUserService = currentUserService;
			_mapper = mapper;
			_createValidator = createValidator;
			_updateValidator = updateValidator;
			_createDetailValidator = createDetailValidator;
			_updateDetailValidator = updateDetailValidator;
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

		public async Task<APIOperationResponse<SupplyDto>> GetByIdAsync(long id)
		{
			_logger.LogInformation("Getting supply by ID. SupplyId: {SupplyId}, User: {UserId}", 
				id, _currentUserService.UserId);

			try
			{
				var supply = await _supplyRepository.FindOneAsync(
					s => s.Id == id && !s.IsDeleted,
					false,
					nameof(Supply.Order),
					$"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}",
					$"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}.{nameof(RequestItem.Item)}",
					nameof(Supply.ReceiverRank),
					$"{nameof(Supply.SupplyDetails)}.{nameof(SupplyDetail.Item)}"
				);

				if (supply == null)
				{
					_logger.LogWarning("Supply not found. SupplyId: {SupplyId}, User: {UserId}", 
						id, _currentUserService.UserId);
					return APIOperationResponse<SupplyDto>.Fail(ResponseType.NotFound, "Supply not found");
				}

				var dto = _mapper.Map<SupplyDto>(supply);
				PopulateSupplyDetailCalculatedProperties(dto, supply);
				
				_logger.LogInformation("Supply retrieved successfully. SupplyId: {SupplyId}, OrderId: {OrderId}", 
					id, supply.OrderId);
				
				return APIOperationResponse<SupplyDto>.Success(dto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting supply by ID. SupplyId: {SupplyId}, User: {UserId}", 
					id, _currentUserService.UserId);
				return APIOperationResponse<SupplyDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<APIOperationResponse<List<SupplyDto>>> GetAllAsync()
		{
			_logger.LogInformation("Getting all supplies. User: {UserId}", _currentUserService.UserId);

			try
			{
				var supplies = await _supplyRepository.FindAsync(
					s => !s.IsDeleted,
					false,
					nameof(Supply.Order),
					$"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}",
					nameof(Supply.ReceiverRank),
					$"{nameof(Supply.SupplyDetails)}.{nameof(SupplyDetail.Item)}"
				);

				var dtos = _mapper.Map<List<SupplyDto>>(supplies);
				
				// Populate calculated properties for each supply
				foreach (var dto in dtos)
				{
					var supply = supplies.FirstOrDefault(s => s.Id == dto.Id);
					if (supply != null)
					{
						PopulateSupplyDetailCalculatedProperties(dto, supply);
					}
				}
				
				_logger.LogInformation("Successfully retrieved {SupplyCount} supplies. User: {UserId}", 
					dtos.Count, _currentUserService.UserId);
				
				return APIOperationResponse<List<SupplyDto>>.Success(dtos);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting all supplies. User: {UserId}", _currentUserService.UserId);
				return APIOperationResponse<List<SupplyDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<APIOperationResponse<long>> CreateAsync(CreateSupplyDto inputDto)
		{
			_logger.LogInformation("Creating new supply. OrderId: {OrderId}, DetailCount: {DetailCount}, User: {UserId}", 
				inputDto?.OrderId, inputDto?.SupplyDetails?.Count ?? 0, _currentUserService.UserId);

			try
			{
				// Validate input
				var validationResult = await _createValidator.ValidateAsync(inputDto);
				if (!validationResult.IsValid)
				{
					var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
					_logger.LogWarning("Supply validation failed. Errors: {ValidationErrors}, User: {UserId}", 
						errors, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
				}

				// Check if order exists
				var order = await _orderRepository.FindOneAsync(
					o => o.Id == inputDto.OrderId && !o.IsDeleted,
					false,
					nameof(Order.RequestItems)
				);

				if (order == null)
				{
					_logger.LogWarning("Order not found. OrderId: {OrderId}, User: {UserId}", 
						inputDto.OrderId, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.NotFound, "Order not found");
				}

				if(order.RequestItems == null || !order.RequestItems.Any())
				{
					_logger.LogWarning("Order has no request items. OrderId: {OrderId}, User: {UserId}", 
						inputDto.OrderId, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Order has no request items");
                }

                // Validate lot and quantity for each supply detail
                var validationErrors = new List<string>();
				foreach (var detailDto in inputDto.SupplyDetails)
				{
					// Find the corresponding request item
					var requestItem = order.RequestItems?.FirstOrDefault(ri => ri.ItemId == detailDto.ItemId && !ri.IsDeleted);
					if (requestItem == null)
					{
						validationErrors.Add($"Item {detailDto.ItemId} is not found in the order");
						continue;
					}

					// Check if quantity exceeds requested quantity
					if (detailDto.Quantity > requestItem.Quantity)
					{
						validationErrors.Add($"Quantity for item {detailDto.ItemId} ({detailDto.Quantity}) cannot exceed requested quantity ({requestItem.Quantity})");
						continue;
					}

					// Validate lot exists and has sufficient quantity
					var lotValidation = await ValidateLotAndQuantityAsync(detailDto.ItemId, detailDto.Lot, detailDto.Quantity);
					if (!lotValidation.IsValid)
					{
						validationErrors.AddRange(lotValidation.Errors);
					}
				}

				if (validationErrors.Any())
				{
					var errorMessage = string.Join("; ", validationErrors);
					_logger.LogWarning("Supply detail validation failed. Errors: {ValidationErrors}, User: {UserId}", 
						errorMessage, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errorMessage);
				}

				// Map DTO to entity
				var supply = _mapper.Map<Supply>(inputDto);
				supply.Status = SupplyStatus.Draft; // Always start with Draft status
				supply.CreationDate = DateTime.UtcNow;
				supply.CreatedBy = _currentUserService.UserId;

				// Map supply details
				supply.SupplyDetails = inputDto.SupplyDetails
					.Select(d =>
					{
						var detail = _mapper.Map<SupplyDetail>(d);
						detail.CreationDate = DateTime.UtcNow;
						detail.CreatedBy = _currentUserService.UserId;
						return detail;
					})
					.ToList();

				// Add to repository
				var createdSupply = await _supplyRepository.AddAsync(supply);

				_logger.LogInformation("Supply created successfully. SupplyId: {SupplyId}, OrderId: {OrderId}, DetailCount: {DetailCount}, User: {UserId}",
					createdSupply.Id, supply.OrderId, supply.SupplyDetails.Count, _currentUserService.UserId);

				return APIOperationResponse<long>.Success(createdSupply.Id, "Supply created successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating supply. OrderId: {OrderId}, User: {UserId}", 
					inputDto?.OrderId, _currentUserService.UserId);
				return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<APIOperationResponse<bool>> UpdateSupplyInfoAsync(long id, UpdateSupplyDto inputDto)
		{
			_logger.LogInformation("Updating supply info. SupplyId: {SupplyId}, User: {UserId}", 
				id, _currentUserService.UserId);

			try
			{
				// Validate input
				var validationResult = await _updateValidator.ValidateAsync(inputDto);
				if (!validationResult.IsValid)
				{
					var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
					_logger.LogWarning("Supply update validation failed. Errors: {ValidationErrors}, User: {UserId}", 
						errors, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
				}

				// Check if supply exists
				var supply = await _supplyRepository.FindOneAsync(s => s.Id == id && !s.IsDeleted);
				if (supply == null)
				{
					_logger.LogWarning("Supply not found. SupplyId: {SupplyId}, User: {UserId}", 
						id, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Supply not found");
				}

				// Check if supply is in draft status (only draft can be modified)
				if (supply.Status != SupplyStatus.Draft)
				{
					_logger.LogWarning("Cannot update supply info. Supply is not in Draft status. SupplyId: {SupplyId}, Status: {Status}, User: {UserId}", 
						id, supply.Status, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
						"Supply info can only be updated when status is Draft");
				}

				// Map updates to entity
				_mapper.Map(inputDto, supply);
				supply.ModificationDate = DateTime.UtcNow;
				supply.ModifiedBy = _currentUserService.UserId;

				await _supplyRepository.UpdateAsync(supply);
				_logger.LogInformation("Supply info updated successfully. SupplyId: {SupplyId}, User: {UserId}", 
					id, _currentUserService.UserId);

				return APIOperationResponse<bool>.Success(true, "Supply info updated successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating supply info. SupplyId: {SupplyId}, User: {UserId}", 
					id, _currentUserService.UserId);
				return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<APIOperationResponse<long>> AddSupplyDetailAsync(long supplyId, CreateSupplyDetailDto detailDto)
		{
			_logger.LogInformation("Adding supply detail. SupplyId: {SupplyId}, ItemId: {ItemId}, Lot: {Lot}, Quantity: {Quantity}, User: {UserId}", 
				supplyId, detailDto.ItemId, detailDto.Lot, detailDto.Quantity, _currentUserService.UserId);

			try
			{
				// Validate input
				var validationResult = await _createDetailValidator.ValidateAsync(detailDto);
				if (!validationResult.IsValid)
				{
					var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
					_logger.LogWarning("Supply detail validation failed. Errors: {ValidationErrors}, User: {UserId}", 
						errors, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
				}

				// Check if supply exists and get order
				var supply = await _supplyRepository.FindOneAsync(
					s => s.Id == supplyId && !s.IsDeleted,
					false,
					nameof(Supply.Order),
					$"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}",
					nameof(Supply.SupplyDetails)
				);

				if (supply == null)
				{
					_logger.LogWarning("Supply not found. SupplyId: {SupplyId}, User: {UserId}", 
						supplyId, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.NotFound, "Supply not found");
				}

				// Check if supply is in draft status
				if (supply.Status != SupplyStatus.Draft)
				{
					_logger.LogWarning("Cannot add supply detail. Supply is not in Draft status. SupplyId: {SupplyId}, Status: {Status}, User: {UserId}", 
						supplyId, supply.Status, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
						"Supply details can only be added when status is Draft");
				}

				// Find the corresponding request item
				var requestItem = supply.Order.RequestItems?.FirstOrDefault(ri => ri.ItemId == detailDto.ItemId && !ri.IsDeleted);
				if (requestItem == null)
				{
					_logger.LogWarning("Item not found in order. SupplyId: {SupplyId}, ItemId: {ItemId}, User: {UserId}", 
						supplyId, detailDto.ItemId, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
						$"Item {detailDto.ItemId} is not found in the order");
				}

				// Calculate total quantity already supplied for this item
				var existingQuantity = supply.SupplyDetails?
					.Where(sd => sd.ItemId == detailDto.ItemId && !sd.IsDeleted)
					.Sum(sd => sd.Quantity) ?? 0;

				// Check if adding this quantity would exceed requested quantity
				if (existingQuantity + detailDto.Quantity > requestItem.Quantity)
				{
					_logger.LogWarning("Quantity exceeds requested quantity. SupplyId: {SupplyId}, ItemId: {ItemId}, Existing: {Existing}, Adding: {Adding}, Requested: {Requested}, User: {UserId}", 
						supplyId, detailDto.ItemId, existingQuantity, detailDto.Quantity, requestItem.Quantity, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
						$"Total quantity for item {detailDto.ItemId} ({existingQuantity + detailDto.Quantity}) cannot exceed requested quantity ({requestItem.Quantity})");
				}

				// Validate lot exists and has sufficient quantity
				var lotValidation = await ValidateLotAndQuantityAsync(detailDto.ItemId, detailDto.Lot, detailDto.Quantity);
				if (!lotValidation.IsValid)
				{
					var errorMessage = string.Join("; ", lotValidation.Errors);
					_logger.LogWarning("Lot validation failed. Errors: {ValidationErrors}, User: {UserId}", 
						errorMessage, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errorMessage);
				}

				// Create new supply detail
				var detail = _mapper.Map<SupplyDetail>(detailDto);
				detail.SupplyId = supplyId;
				detail.CreationDate = DateTime.UtcNow;
				detail.CreatedBy = _currentUserService.UserId;

				var createdDetail = await _supplyDetailRepository.AddAsync(detail);
				_logger.LogInformation("Supply detail added successfully. SupplyId: {SupplyId}, DetailId: {DetailId}, ItemId: {ItemId}, User: {UserId}", 
					supplyId, createdDetail.Id, detailDto.ItemId, _currentUserService.UserId);

				return APIOperationResponse<long>.Success(createdDetail.Id, "Supply detail added successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding supply detail. SupplyId: {SupplyId}, ItemId: {ItemId}, User: {UserId}", 
					supplyId, detailDto.ItemId, _currentUserService.UserId);
				return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<APIOperationResponse<bool>> UpdateSupplyDetailAsync(long supplyId, long detailId, UpdateSupplyDetailDto detailDto)
		{
			_logger.LogInformation("Updating supply detail. SupplyId: {SupplyId}, DetailId: {DetailId}, ItemId: {ItemId}, Lot: {Lot}, Quantity: {Quantity}, User: {UserId}", 
				supplyId, detailId, detailDto.ItemId, detailDto.Lot, detailDto.Quantity, _currentUserService.UserId);

			try
			{
				// Validate input
				var validationResult = await _updateDetailValidator.ValidateAsync(detailDto);
				if (!validationResult.IsValid)
				{
					var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
					_logger.LogWarning("Supply detail update validation failed. Errors: {ValidationErrors}, User: {UserId}", 
						errors, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
				}

				// Check if supply exists and get order
				var supply = await _supplyRepository.FindOneAsync(
					s => s.Id == supplyId && !s.IsDeleted,
					false,
					nameof(Supply.Order),
					$"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}",
					nameof(Supply.SupplyDetails)
				);

				if (supply == null)
				{
					_logger.LogWarning("Supply not found. SupplyId: {SupplyId}, User: {UserId}", 
						supplyId, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Supply not found");
				}

				// Check if supply is in draft status
				if (supply.Status != SupplyStatus.Draft)
				{
					_logger.LogWarning("Cannot update supply detail. Supply is not in Draft status. SupplyId: {SupplyId}, Status: {Status}, User: {UserId}", 
						supplyId, supply.Status, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
						"Supply details can only be updated when status is Draft");
				}

				// Find the supply detail
				var detail = supply.SupplyDetails?.FirstOrDefault(sd => sd.Id == detailId && !sd.IsDeleted);
				if (detail == null)
				{
					_logger.LogWarning("Supply detail not found. SupplyId: {SupplyId}, DetailId: {DetailId}, User: {UserId}", 
						supplyId, detailId, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Supply detail not found");
				}

				// Find the corresponding request item
				var requestItem = supply.Order.RequestItems?.FirstOrDefault(ri => ri.ItemId == detailDto.ItemId && !ri.IsDeleted);
				if (requestItem == null)
				{
					_logger.LogWarning("Item not found in order. SupplyId: {SupplyId}, ItemId: {ItemId}, User: {UserId}", 
						supplyId, detailDto.ItemId, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
						$"Item {detailDto.ItemId} is not found in the order");
				}

				// Calculate total quantity already supplied for this item (excluding current detail)
				var existingQuantity = supply.SupplyDetails?
					.Where(sd => sd.ItemId == detailDto.ItemId && sd.Id != detailId && !sd.IsDeleted)
					.Sum(sd => sd.Quantity) ?? 0;

				// Check if updating this quantity would exceed requested quantity
				if (existingQuantity + detailDto.Quantity > requestItem.Quantity)
				{
					_logger.LogWarning("Quantity exceeds requested quantity. SupplyId: {SupplyId}, ItemId: {ItemId}, Existing: {Existing}, Updating: {Updating}, Requested: {Requested}, User: {UserId}", 
						supplyId, detailDto.ItemId, existingQuantity, detailDto.Quantity, requestItem.Quantity, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
						$"Total quantity for item {detailDto.ItemId} ({existingQuantity + detailDto.Quantity}) cannot exceed requested quantity ({requestItem.Quantity})");
				}

				// Validate lot exists and has sufficient quantity
				var lotValidation = await ValidateLotAndQuantityAsync(detailDto.ItemId, detailDto.Lot, detailDto.Quantity);
				if (!lotValidation.IsValid)
				{
					var errorMessage = string.Join("; ", lotValidation.Errors);
					_logger.LogWarning("Lot validation failed. Errors: {ValidationErrors}, User: {UserId}", 
						errorMessage, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errorMessage);
				}

				// Update detail
				_mapper.Map(detailDto, detail);
				detail.ModificationDate = DateTime.UtcNow;
				detail.ModifiedBy = _currentUserService.UserId;

				await _supplyDetailRepository.UpdateAsync(detail);
				_logger.LogInformation("Supply detail updated successfully. SupplyId: {SupplyId}, DetailId: {DetailId}, User: {UserId}", 
					supplyId, detailId, _currentUserService.UserId);

				return APIOperationResponse<bool>.Success(true, "Supply detail updated successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating supply detail. SupplyId: {SupplyId}, DetailId: {DetailId}, User: {UserId}", 
					supplyId, detailId, _currentUserService.UserId);
				return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<APIOperationResponse<bool>> DeleteSupplyDetailAsync(long supplyId, long detailId)
		{
			_logger.LogInformation("Deleting supply detail. SupplyId: {SupplyId}, DetailId: {DetailId}, User: {UserId}", 
				supplyId, detailId, _currentUserService.UserId);

			try
			{
				// Check if supply exists
				var supply = await _supplyRepository.FindOneAsync(
					s => s.Id == supplyId && !s.IsDeleted,
					false,
					nameof(Supply.SupplyDetails)
				);

				if (supply == null)
				{
					_logger.LogWarning("Supply not found. SupplyId: {SupplyId}, User: {UserId}", 
						supplyId, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Supply not found");
				}

				// Check if supply is in draft status
				if (supply.Status != SupplyStatus.Draft)
				{
					_logger.LogWarning("Cannot delete supply detail. Supply is not in Draft status. SupplyId: {SupplyId}, Status: {Status}, User: {UserId}", 
						supplyId, supply.Status, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
						"Supply details can only be deleted when status is Draft");
				}

				// Count active details in the supply
				var activeDetailsCount = supply.SupplyDetails?.Count(sd => !sd.IsDeleted) ?? 0;

				if (activeDetailsCount <= 1)
				{
					_logger.LogWarning("Cannot delete last detail from supply. SupplyId: {SupplyId}, DetailId: {DetailId}, User: {UserId}", 
						supplyId, detailId, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
						"Cannot delete the last detail from a supply. A supply must have at least one detail.");
				}

				// Find the supply detail to delete
				var detail = supply.SupplyDetails?.FirstOrDefault(sd => sd.Id == detailId && !sd.IsDeleted);
				if (detail == null)
				{
					_logger.LogWarning("Supply detail not found. SupplyId: {SupplyId}, DetailId: {DetailId}, User: {UserId}", 
						supplyId, detailId, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Supply detail not found");
				}

				// Soft delete the detail
				await _supplyDetailRepository.DeleteAsync(detail);
				_logger.LogInformation("Supply detail deleted successfully. SupplyId: {SupplyId}, DetailId: {DetailId}, User: {UserId}", 
					supplyId, detailId, _currentUserService.UserId);

				return APIOperationResponse<bool>.Success(true, "Supply detail deleted successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting supply detail. SupplyId: {SupplyId}, DetailId: {DetailId}, User: {UserId}", 
					supplyId, detailId, _currentUserService.UserId);
				return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<APIOperationResponse<bool>> UpdateStatusAsync(long id, SupplyStatus newStatus)
		{
			_logger.LogInformation("Updating supply status. SupplyId: {SupplyId}, NewStatus: {NewStatus}, User: {UserId}", 
				id, newStatus, _currentUserService.UserId);

			try
			{
				// Check if supply exists
				var supply = await _supplyRepository.FindOneAsync(
					s => s.Id == id && !s.IsDeleted,
					false,
					nameof(Supply.Order),
					$"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}",
					nameof(Supply.SupplyDetails)
				);

				if (supply == null)
				{
					_logger.LogWarning("Supply not found. SupplyId: {SupplyId}, User: {UserId}", 
						id, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Supply not found");
				}

				// Validate status transition
				var validationResult = ValidateStatusTransition(supply.Status, newStatus);
				if (!validationResult.IsValid)
				{
					_logger.LogWarning("Invalid status transition. SupplyId: {SupplyId}, CurrentStatus: {CurrentStatus}, NewStatus: {NewStatus}, Error: {Error}, User: {UserId}", 
						id, supply.Status, newStatus, validationResult.ErrorMessage, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, validationResult.ErrorMessage);
				}

				// Auto-calculate status if transitioning to Completed
				if (newStatus == SupplyStatus.Completed)
				{
					var calculatedStatus = CalculateSupplyStatus(supply);
					if (calculatedStatus != SupplyStatus.Completed)
					{
						_logger.LogWarning("Cannot set status to Completed. Supply is not fully fulfilled. SupplyId: {SupplyId}, CalculatedStatus: {CalculatedStatus}, User: {UserId}", 
							id, calculatedStatus, _currentUserService.UserId);
						return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
							$"Cannot set status to Completed. Supply is currently {calculatedStatus}. All items must be fully supplied.");
					}
				}

				var oldStatus = supply.Status;
				supply.Status = newStatus;
				supply.ModificationDate = DateTime.UtcNow;
				supply.ModifiedBy = _currentUserService.UserId;

				await _supplyRepository.UpdateAsync(supply);
				_logger.LogInformation("Supply status updated successfully. SupplyId: {SupplyId}, OldStatus: {OldStatus}, NewStatus: {NewStatus}, User: {UserId}", 
					id, oldStatus, newStatus, _currentUserService.UserId);

				return APIOperationResponse<bool>.Success(true, "Supply status updated successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating supply status. SupplyId: {SupplyId}, NewStatus: {NewStatus}, User: {UserId}", 
					id, newStatus, _currentUserService.UserId);
				return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}

		/// <summary>
		/// Validates that a lot exists and has sufficient quantity available
		/// </summary>
		private async Task<(bool IsValid, List<string> Errors)> ValidateLotAndQuantityAsync(long itemId, int lot, long requestedQuantity)
		{
			var errors = new List<string>();

			// Get inventory detail for this lot
			var inventoryDetail = await _inventoryDetailRepository.FindOneAsync(
				id => id.ItemId == itemId && id.Lot == lot,
				false,
				nameof(InventoryDetail.Inventory)
			);

			if (inventoryDetail == null)
			{
				errors.Add($"Lot {lot} does not exist for item {itemId}");
				return (false, errors);
			}

			// Check if inventory is deleted
			if (inventoryDetail.Inventory?.IsDeleted == true)
			{
				errors.Add($"Lot {lot} belongs to a deleted inventory");
				return (false, errors);
			}

			// Get all supply details for this lot to calculate used quantity
			var supplyDetails = await _supplyDetailRepository.FindAsync(
				sd => sd.ItemId == itemId && sd.Lot == lot
			);

			var totalUsedQuantity = supplyDetails.Sum(sd => sd.Quantity);
			var availableQuantity = inventoryDetail.ItemQuantity - totalUsedQuantity;

			if (availableQuantity < requestedQuantity)
			{
				errors.Add($"Lot {lot} for item {itemId} has insufficient quantity. Available: {availableQuantity}, Requested: {requestedQuantity}");
				return (false, errors);
			}

			return (true, errors);
		}

		/// <summary>
		/// Validates status transition rules
		/// </summary>
		private (bool IsValid, string ErrorMessage) ValidateStatusTransition(SupplyStatus currentStatus, SupplyStatus newStatus)
		{
			// Same status is always valid
			if (currentStatus == newStatus)
				return (true, string.Empty);

			 
			var validTransitions = new Dictionary<SupplyStatus, List<SupplyStatus>>
			{
				{ SupplyStatus.Draft, new List<SupplyStatus> { SupplyStatus.Partial, SupplyStatus.Completed, SupplyStatus.Cancelled } },
				{ SupplyStatus.Partial, new List<SupplyStatus> { SupplyStatus.Completed, SupplyStatus.Cancelled } },
				{ SupplyStatus.Completed, new List<SupplyStatus>() }, // Cannot transition from Completed
				{ SupplyStatus.Cancelled, new List<SupplyStatus>() } // Cannot transition from Cancelled
			};

			if (!validTransitions.ContainsKey(currentStatus))
			{
				return (false, $"Unknown current status: {currentStatus}");
			}

			if (!validTransitions[currentStatus].Contains(newStatus))
			{
				return (false, $"Cannot transition from {currentStatus} to {newStatus}. Valid transitions: {string.Join(", ", validTransitions[currentStatus])}");
			}

			return (true, string.Empty);
		}

		/// <summary>
		/// Calculates the supply status based on supplied quantities vs requested quantities
		/// </summary>
		private SupplyStatus CalculateSupplyStatus(Supply supply)
		{
			if (supply.SupplyDetails == null || !supply.SupplyDetails.Any(sd => !sd.IsDeleted))
			{
				return SupplyStatus.Draft;
			}

			if (supply.Order?.RequestItems == null || !supply.Order.RequestItems.Any(ri => !ri.IsDeleted))
			{
				return SupplyStatus.Draft;
			}

			// Group supply details by item
			var supplyByItem = supply.SupplyDetails
				.Where(sd => !sd.IsDeleted)
				.GroupBy(sd => sd.ItemId)
				.ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

			// Check each request item
			bool allCompleted = true;
			bool anySupplied = false;

			foreach (var requestItem in supply.Order.RequestItems.Where(ri => !ri.IsDeleted))
			{
				var suppliedQuantity = supplyByItem.TryGetValue(requestItem.ItemId, out var qty) ? qty : 0;

				if (suppliedQuantity > 0)
					anySupplied = true;

				if (suppliedQuantity < requestItem.Quantity)
				{
					allCompleted = false;
				}
			}

			if (allCompleted && anySupplied)
				return SupplyStatus.Completed;
			else if (anySupplied)
				return SupplyStatus.Partial;
			else
				return SupplyStatus.Draft;
		}

		/// <summary>
		/// Populates calculated properties for SupplyDetailDto objects
		/// </summary>
		private void PopulateSupplyDetailCalculatedProperties(SupplyDto supplyDto, Supply supply)
		{
			if (supplyDto.SupplyDetails == null || supply.SupplyDetails == null || supply.Order?.RequestItems == null)
				return;

			// Group supply details by item to calculate total supplied quantity per item
			var totalSuppliedByItem = supply.SupplyDetails
				.Where(sd => !sd.IsDeleted)
				.GroupBy(sd => sd.ItemId)
				.ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

			// Get requested quantities from order items
			var requestedByItem = supply.Order.RequestItems
				.Where(ri => !ri.IsDeleted)
				.ToDictionary(ri => ri.ItemId, ri => ri.Quantity);

			// Populate calculated properties for each supply detail
			foreach (var detailDto in supplyDto.SupplyDetails)
			{
				// Get requested quantity for this item
				detailDto.RequestedQuantity = requestedByItem.TryGetValue(detailDto.ItemId, out var requestedQty) 
					? requestedQty 
					: 0;

				// Get total supplied quantity for this item
				detailDto.TotalSuppliedQuantity = totalSuppliedByItem.TryGetValue(detailDto.ItemId, out var totalSupplied) 
					? totalSupplied 
					: 0;

				// Check if fully fulfilled
				detailDto.IsFullyFulfilled = detailDto.TotalSuppliedQuantity >= detailDto.RequestedQuantity;
			}
		}
	}
}

