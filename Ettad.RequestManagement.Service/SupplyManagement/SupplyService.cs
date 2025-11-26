using AutoMapper;
using FluentValidation;
using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Inventories;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.Notification.Service;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Identity;
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
		private readonly IValidator<SubmitSupplyDto> _submitValidator;
		private readonly IValidator<SetSupplyPickupDateDto> _setPickupDateValidator;
		private readonly IValidator<ConfirmSupplyPickupDateDto> _confirmPickupDateValidator;
		private readonly INotificationHelperService _notificationHelperService;
		private readonly UserManager<ApplicationUser> _userManager;
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
			IValidator<SubmitSupplyDto> submitValidator,
			IValidator<SetSupplyPickupDateDto> setPickupDateValidator,
			IValidator<ConfirmSupplyPickupDateDto> confirmPickupDateValidator,
			INotificationHelperService notificationHelperService,
			UserManager<ApplicationUser> userManager,
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
			_submitValidator = submitValidator;
			_setPickupDateValidator = setPickupDateValidator;
			_confirmPickupDateValidator = confirmPickupDateValidator;
			_notificationHelperService = notificationHelperService;
			_userManager = userManager;
			_logger = logger;
		}

		public async Task<APIOperationResponse<OrderSupplySuggestionDto>> GetSupplySuggestionAsync(long orderId, List<long>? depotIds = null)
		{
			_logger.LogInformation("Getting supply suggestion for order. OrderId: {OrderId}, DepotIds: {DepotIds}, User: {UserId}", 
				orderId, depotIds != null ? string.Join(", ", depotIds) : "All", _currentUserService.UserId);

			try
			{
				var result = await _inventoryService.SuggestSupplyForOrderAsync(orderId, depotIds);

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

		public async Task<APIOperationResponse<SupplyDto>> GetByOrderIdAsync(long orderId)
		{
            _logger.LogInformation("Getting supply by Order ID. OrderID: {OrderID}, User: {UserId}",
                orderId, _currentUserService.UserId);

            try
            {
                var supply = await _supplyRepository.FindOneAsync(
                    s => s.OrderId == orderId && !s.IsDeleted,
                    false,
                    nameof(Supply.Order),
                    $"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}",
                    $"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}.{nameof(RequestItem.Item)}",
                    nameof(Supply.ReceiverRank),
                    $"{nameof(Supply.SupplyDetails)}.{nameof(SupplyDetail.Item)}"
                );

                if (supply == null)
                {
                    _logger.LogWarning("Supply not found. OrderID: {OrderID}, User: {UserId}",
                        orderId, _currentUserService.UserId);
                    return APIOperationResponse<SupplyDto>.Fail(ResponseType.NotFound, "Supply not found");
                }

                var dto = _mapper.Map<SupplyDto>(supply);
                PopulateSupplyDetailCalculatedProperties(dto, supply);

                _logger.LogInformation("Supply retrieved successfully. OrderID: {OrderID}, OrderId: {OrderId}",
                    orderId, supply.OrderId);

                return APIOperationResponse<SupplyDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting supply by Order ID. OrderID: {OrderID}, User: {UserId}",
                    orderId, _currentUserService.UserId);
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

				// Check if order exists and load RequestItems for fulfillment calculation
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

				// Check if there's a Draft supply for this order - if yes, suggest updating instead
				var existingDraftSupply = await _supplyRepository.FindOneAsync(
					s => s.OrderId == inputDto.OrderId && !s.IsDeleted && s.SubmissionStatus == SupplySubmissionStatus.Draft
				);

				if (existingDraftSupply != null)
				{
					_logger.LogWarning("Draft supply already exists for order. OrderId: {OrderId}, SupplyId: {SupplyId}, User: {UserId}", 
						inputDto.OrderId, existingDraftSupply.Id, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
						$"A Draft supply already exists for this order (Supply ID: {existingDraftSupply.Id}). Please update the existing supply instead of creating a new one.");
				}

				// Get all existing supplies for this order (excluding deleted)
				var existingSupplies = await _supplyRepository.FindAsync(
					s => s.OrderId == inputDto.OrderId && !s.IsDeleted,
					false,
					nameof(Supply.SupplyDetails)
				);

				// Calculate already supplied quantities per item
				var alreadySuppliedQuantities = await CalculateAlreadySuppliedQuantitiesAsync(inputDto.OrderId);

				// Business Rule: Can have multiple supplies if order is partially supplied AND status is Submitted
				// Validate fulfillment status consistency - all supplies should have same fulfillment status
				if (existingSupplies.Any())
				{
					var existingFulfillmentStatuses = existingSupplies.Select(s => s.FulfillmentStatus).Distinct().ToList();
					if (existingFulfillmentStatuses.Count > 1)
					{
						var statusesStr = string.Join(", ", existingFulfillmentStatuses);
						_logger.LogWarning("Inconsistent supply fulfillment statuses found. OrderId: {OrderId}, Statuses: {Statuses}, User: {UserId}", 
							inputDto.OrderId, statusesStr, _currentUserService.UserId);
						return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
							$"Inconsistent supply fulfillment statuses found for this order: {statusesStr}. All supplies must have the same fulfillment status (Partial or Fully).");
					}

					var existingFulfillmentStatus = existingFulfillmentStatuses.First();
					
					// Check if all existing supplies are Submitted
					var allSubmitted = existingSupplies.All(s => s.SubmissionStatus == SupplySubmissionStatus.Submitted);
					
					// Can only create new supply if: Partial fulfillment AND all existing supplies are Submitted
					if (existingFulfillmentStatus != SupplyFulfillmentStatus.Partial || !allSubmitted)
					{
						_logger.LogWarning("Cannot create new supply. OrderId: {OrderId}, FulfillmentStatus: {FulfillmentStatus}, AllSubmitted: {AllSubmitted}, User: {UserId}", 
							inputDto.OrderId, existingFulfillmentStatus, allSubmitted, _currentUserService.UserId);
						return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
							$"Cannot create new supply. Order fulfillment status is '{existingFulfillmentStatus}'. New supplies can only be created when order is Partially fulfilled and all existing supplies are Submitted.");
					}
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

					// Calculate total supplied quantity (existing + new)
					var alreadySupplied = alreadySuppliedQuantities.TryGetValue(detailDto.ItemId, out var supplied) ? supplied : 0;
					var totalSuppliedAfterThis = alreadySupplied + detailDto.Quantity;

					// Check if total supplied quantity exceeds requested quantity
					if (totalSuppliedAfterThis > requestItem.Quantity)
					{
						validationErrors.Add($"Total quantity for item {detailDto.ItemId} ({totalSuppliedAfterThis}) cannot exceed requested quantity ({requestItem.Quantity}). Already supplied: {alreadySupplied}, New: {detailDto.Quantity}");
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
				supply.SubmissionStatus = SupplySubmissionStatus.Draft; // Always start with Draft submission status
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

                // Set Order navigation property for fulfillment calculation, and set it to null after
                supply.Order = order;

				// Calculate fulfillment status based on supplied quantities vs requested quantities
				supply.FulfillmentStatus = CalculateFulfillmentStatus(supply);

                // Clear Order navigation property to avoid unintended data persistence
				supply.Order = null;

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

				// Business Rule: Can modify if Draft submission status AND Fully fulfilled
				// Or if Draft submission status (regardless of fulfillment)
				if (supply.SubmissionStatus != SupplySubmissionStatus.Draft)
				{
					_logger.LogWarning("Cannot update supply info. Supply is not in Draft submission status. SupplyId: {SupplyId}, SubmissionStatus: {SubmissionStatus}, FulfillmentStatus: {FulfillmentStatus}, User: {UserId}", 
						id, supply.SubmissionStatus, supply.FulfillmentStatus, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
						"Supply info can only be updated when submission status is Draft");
				}
				
				// Additional check: If Fully fulfilled and Draft, can modify
				// If Submitted, cannot modify (already checked above)
				if (supply.FulfillmentStatus == SupplyFulfillmentStatus.Fully && supply.SubmissionStatus == SupplySubmissionStatus.Draft)
				{
					_logger.LogInformation("Updating Fully fulfilled Draft supply. SupplyId: {SupplyId}, User: {UserId}", 
						id, _currentUserService.UserId);
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

				// Business Rule: Can modify details only if Draft submission status
				if (supply.SubmissionStatus != SupplySubmissionStatus.Draft)
				{
					_logger.LogWarning("Cannot add supply detail. Supply is not in Draft submission status. SupplyId: {SupplyId}, SubmissionStatus: {SubmissionStatus}, User: {UserId}", 
						supplyId, supply.SubmissionStatus, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
						"Supply details can only be added when submission status is Draft");
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

				// Calculate total quantity already supplied for this item (from this supply + other supplies)
				var existingQuantityInThisSupply = supply.SupplyDetails?
					.Where(sd => sd.ItemId == detailDto.ItemId && !sd.IsDeleted)
					.Sum(sd => sd.Quantity) ?? 0;

				// Get already supplied quantities from other supplies for this order (excluding this supply)
				var alreadySuppliedFromOtherSupplies = await CalculateAlreadySuppliedQuantitiesAsync(supply.OrderId, supplyId);
				var alreadySuppliedFromOthers = alreadySuppliedFromOtherSupplies.TryGetValue(detailDto.ItemId, out var othersSupplied) ? othersSupplied : 0;

				var totalSuppliedAfterAdding = alreadySuppliedFromOthers + existingQuantityInThisSupply + detailDto.Quantity;

				// Check if adding this quantity would exceed requested quantity
				if (totalSuppliedAfterAdding > requestItem.Quantity)
				{
					_logger.LogWarning("Quantity exceeds requested quantity. SupplyId: {SupplyId}, ItemId: {ItemId}, AlreadyFromOthers: {Others}, ExistingInThis: {Existing}, Adding: {Adding}, Requested: {Requested}, User: {UserId}", 
						supplyId, detailDto.ItemId, alreadySuppliedFromOthers, existingQuantityInThisSupply, detailDto.Quantity, requestItem.Quantity, _currentUserService.UserId);
					return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
						$"Total quantity for item {detailDto.ItemId} ({totalSuppliedAfterAdding}) cannot exceed requested quantity ({requestItem.Quantity}). Already supplied from other supplies: {alreadySuppliedFromOthers}, In this supply: {existingQuantityInThisSupply}, Adding: {detailDto.Quantity}");
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

				// Recalculate fulfillment status after adding detail
				// Reload supply with details to recalculate
				var updatedSupply = await _supplyRepository.FindOneAsync(
					s => s.Id == supplyId && !s.IsDeleted,
					false,
					nameof(Supply.Order),
					$"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}",
					nameof(Supply.SupplyDetails)
				);
				if (updatedSupply != null)
				{
					updatedSupply.FulfillmentStatus = CalculateFulfillmentStatus(updatedSupply);
					updatedSupply.ModificationDate = DateTime.UtcNow;
					updatedSupply.ModifiedBy = _currentUserService.UserId;
					await _supplyRepository.UpdateAsync(updatedSupply);
				}

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

				// Business Rule: Can modify details only if Draft submission status
				if (supply.SubmissionStatus != SupplySubmissionStatus.Draft)
				{
					_logger.LogWarning("Cannot update supply detail. Supply is not in Draft submission status. SupplyId: {SupplyId}, SubmissionStatus: {SubmissionStatus}, User: {UserId}", 
						supplyId, supply.SubmissionStatus, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
						"Supply details can only be updated when submission status is Draft");
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
				var existingQuantityInThisSupply = supply.SupplyDetails?
					.Where(sd => sd.ItemId == detailDto.ItemId && sd.Id != detailId && !sd.IsDeleted)
					.Sum(sd => sd.Quantity) ?? 0;

				// Get already supplied quantities from other supplies for this order (excluding this supply)
				var alreadySuppliedFromOtherSupplies = await CalculateAlreadySuppliedQuantitiesAsync(supply.OrderId, supplyId);
				var alreadySuppliedFromOthers = alreadySuppliedFromOtherSupplies.TryGetValue(detailDto.ItemId, out var othersSupplied) ? othersSupplied : 0;

				var totalSuppliedAfterUpdating = alreadySuppliedFromOthers + existingQuantityInThisSupply + detailDto.Quantity;

				// Check if updating this quantity would exceed requested quantity
				if (totalSuppliedAfterUpdating > requestItem.Quantity)
				{
					_logger.LogWarning("Quantity exceeds requested quantity. SupplyId: {SupplyId}, ItemId: {ItemId}, AlreadyFromOthers: {Others}, ExistingInThis: {Existing}, Updating: {Updating}, Requested: {Requested}, User: {UserId}", 
						supplyId, detailDto.ItemId, alreadySuppliedFromOthers, existingQuantityInThisSupply, detailDto.Quantity, requestItem.Quantity, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
						$"Total quantity for item {detailDto.ItemId} ({totalSuppliedAfterUpdating}) cannot exceed requested quantity ({requestItem.Quantity}). Already supplied from other supplies: {alreadySuppliedFromOthers}, In this supply: {existingQuantityInThisSupply}, Updating: {detailDto.Quantity}");
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

				// Recalculate fulfillment status after updating detail
				// Reload supply with details to recalculate
				var updatedSupply = await _supplyRepository.FindOneAsync(
					s => s.Id == supplyId && !s.IsDeleted,
					false,
					nameof(Supply.Order),
					$"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}",
					nameof(Supply.SupplyDetails)
				);
				if (updatedSupply != null)
				{
					updatedSupply.FulfillmentStatus = CalculateFulfillmentStatus(updatedSupply);
					updatedSupply.ModificationDate = DateTime.UtcNow;
					updatedSupply.ModifiedBy = _currentUserService.UserId;
					await _supplyRepository.UpdateAsync(updatedSupply);
				}

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

				// Business Rule: Can modify details only if Draft submission status
				if (supply.SubmissionStatus != SupplySubmissionStatus.Draft)
				{
					_logger.LogWarning("Cannot delete supply detail. Supply is not in Draft submission status. SupplyId: {SupplyId}, SubmissionStatus: {SubmissionStatus}, User: {UserId}", 
						supplyId, supply.SubmissionStatus, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
						"Supply details can only be deleted when submission status is Draft");
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

				// Recalculate fulfillment status after deleting detail
				// Reload supply with details to recalculate
				var updatedSupply = await _supplyRepository.FindOneAsync(
					s => s.Id == supplyId && !s.IsDeleted,
					false,
					nameof(Supply.Order),
					$"{nameof(Supply.Order)}.{nameof(Order.RequestItems)}",
					nameof(Supply.SupplyDetails)
				);
				if (updatedSupply != null)
				{
					updatedSupply.FulfillmentStatus = CalculateFulfillmentStatus(updatedSupply);
					updatedSupply.ModificationDate = DateTime.UtcNow;
					updatedSupply.ModifiedBy = _currentUserService.UserId;
					await _supplyRepository.UpdateAsync(updatedSupply);
				}

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

		public async Task<APIOperationResponse<bool>> SubmitSupplyAsync(long id, SubmitSupplyDto inputDto)
		{
			_logger.LogInformation("Submitting supply. SupplyId: {SupplyId}, User: {UserId}",
				id, _currentUserService.UserId);

			try
			{
				var validationResult = await _submitValidator.ValidateAsync(inputDto);
				if (!validationResult.IsValid)
				{
					var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
					_logger.LogWarning("Supply submission validation failed. Errors: {ValidationErrors}, User: {UserId}",
						errors, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
				}

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

				if (supply.SubmissionStatus == SupplySubmissionStatus.Submitted)
				{
					_logger.LogWarning("Supply already submitted. SupplyId: {SupplyId}, User: {UserId}",
						id, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Supply is already submitted.");
				}

				if (supply.SupplyDetails == null || !supply.SupplyDetails.Any(sd => !sd.IsDeleted))
				{
					_logger.LogWarning("Cannot submit supply without details. SupplyId: {SupplyId}, User: {UserId}",
						id, _currentUserService.UserId);
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Supply must have at least one detail before submission.");
				}

				// Update receiver information and submission metadata
				supply.RecieverName = inputDto.RecieverName;
				supply.ReceiverRankId = inputDto.ReceiverRankId;
				supply.RecieverMilitaryId = inputDto.RecieverMilitaryId;
				supply.Notes = inputDto.Notes;
				supply.SubmissionStatus = SupplySubmissionStatus.Submitted;
				supply.FulfillmentStatus = CalculateFulfillmentStatus(supply);
				supply.ModificationDate = DateTime.UtcNow;
				supply.ModifiedBy = _currentUserService.UserId;

				await _supplyRepository.UpdateAsync(supply);
				_logger.LogInformation("Supply submitted successfully. SupplyId: {SupplyId}, User: {UserId}",
					id, _currentUserService.UserId);

				return APIOperationResponse<bool>.Success(true, "Supply submitted successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error submitting supply. SupplyId: {SupplyId}, User: {UserId}",
					id, _currentUserService.UserId);
				return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}


		/// <summary>
		/// Calculates already supplied quantities per item for an order
		/// </summary>
		/// <param name="orderId">The order ID</param>
		/// <param name="excludeSupplyId">Optional supply ID to exclude from calculation (to avoid double counting)</param>
		private async Task<Dictionary<long, long>> CalculateAlreadySuppliedQuantitiesAsync(long orderId, long? excludeSupplyId = null)
		{
			// Get all existing supplies for this order (excluding deleted)
			var existingSupplies = await _supplyRepository.FindAsync(
				s => s.OrderId == orderId && !s.IsDeleted,
				false,
				nameof(Supply.SupplyDetails)
			);

			// Get all supply details for these supplies (excluding the specified supply if provided)
			var supplyIds = existingSupplies
				.Where(s => !excludeSupplyId.HasValue || s.Id != excludeSupplyId.Value)
				.Select(s => s.Id)
				.ToList();

			if (!supplyIds.Any())
			{
				return new Dictionary<long, long>();
			}

			var existingSupplyDetails = await _supplyDetailRepository.FindAsync(
				sd => supplyIds.Contains(sd.SupplyId) && !sd.IsDeleted
			);

			// Calculate already supplied quantities per item
			return existingSupplyDetails
				.GroupBy(sd => sd.ItemId)
				.ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));
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
		/// Calculates the supply fulfillment status based on supplied quantities vs requested quantities
		/// </summary>
		private SupplyFulfillmentStatus CalculateFulfillmentStatus(Supply supply)
		{
			if (supply.SupplyDetails == null || !supply.SupplyDetails.Any(sd => !sd.IsDeleted))
			{
				return SupplyFulfillmentStatus.Partial;
			}

			if (supply.Order?.RequestItems == null || !supply.Order.RequestItems.Any(ri => !ri.IsDeleted))
			{
				return SupplyFulfillmentStatus.Partial;
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
				return SupplyFulfillmentStatus.Fully;
			else
				return SupplyFulfillmentStatus.Partial;
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

		public async Task<APIOperationResponse<bool>> SetSupplyPickupDateAsync(long orderId, SetSupplyPickupDateDto inputDto)
		{
			try
			{
				// Validate input
				var validationResult = await _setPickupDateValidator.ValidateAsync(inputDto);
				if (!validationResult.IsValid)
				{
					var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
				}

				// Check if supply exists and load Order with Requester
				var supply = await _supplyRepository.FindOneAsync(
					s => s.OrderId == orderId && !s.IsDeleted && s.SubmissionStatus == SupplySubmissionStatus.Draft,
					false,
					nameof(Supply.Order),
					$"{nameof(Supply.Order)}.{nameof(Order.Requester)}"
				);
				if (supply == null)
				{
					return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Draft supply not found for this order");
				}

				// Update only the SupplyDate field
				supply.SupplyDate = inputDto.SupplyDate;
				supply.ModificationDate = DateTime.UtcNow;
				supply.ModifiedBy = _currentUserService.UserId;

				await _supplyRepository.UpdateAsync(supply);

				// Notify the order requester
				if (supply.Order?.RequesterId != null)
				{
					try
					{
						// Get current user details for contact information
						var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId ?? string.Empty);
						var currentUserName = currentUser?.FullNameEN ?? currentUser?.UserName ?? "the administrator";
						var currentUserEmail = currentUser?.Email;
						var currentUserPhone = currentUser?.PhoneNumber;

						// Build contact information string
						var contactParts = new List<string>();
						if (!string.IsNullOrEmpty(currentUserEmail))
							contactParts.Add($"email: {currentUserEmail}");
						if (!string.IsNullOrEmpty(currentUserPhone))
							contactParts.Add($"phone: {currentUserPhone}");

						var contactInfo = contactParts.Any()
							? $"If this date is not suitable, please contact {currentUserName} ({string.Join(" or ", contactParts)}) to arrange an alternative."
							: $"If this date is not suitable, please contact {currentUserName} to arrange an alternative.";

						await _notificationHelperService.SendNotificationAsync(
							title: "Supply Pickup Date Set",
							message: $"The supply pickup date for Order #{supply.Order.RequestNo} has been set to {inputDto.SupplyDate:yyyy-MM-dd}. {contactInfo}",
							entityType: "Supply",
							entityId: supply.Id,
							userIds: new List<string> { supply.Order.RequesterId },
							senderId: _currentUserService.UserId
						);
					}
					catch (Exception ex)
					{
						// Log error but don't fail the operation - notifications are non-critical
						_logger.LogError(ex, "Error sending notification to requester. SupplyId: {SupplyId}, RequesterId: {RequesterId}",
							supply.Id, supply.Order.RequesterId);
					}
				}

				return APIOperationResponse<bool>.Success(true, "Supply pickup date set successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error setting supply pickup date. OrderId: {OrderId}, User: {UserId}",
					orderId, _currentUserService.UserId);
				return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<APIOperationResponse<bool>> ConfirmSupplyPickupDateAsync(long orderId, ConfirmSupplyPickupDateDto inputDto)
		{
			try
			{
				// Validate input
				var validationResult = await _confirmPickupDateValidator.ValidateAsync(inputDto);
				if (!validationResult.IsValid)
				{
					var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
					return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
				}

				// Check if supply exists and load Order with Requester
				var supply = await _supplyRepository.FindOneAsync(
					s => s.OrderId == orderId && !s.IsDeleted && s.SubmissionStatus == SupplySubmissionStatus.Draft,
					false,
					nameof(Supply.Order),
					$"{nameof(Supply.Order)}.{nameof(Order.Requester)}"
				);
				if (supply == null)
				{
					return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Draft supply not found for this order");
				}

				// Update only the SupplyDate field
				supply.SupplyDate = inputDto.SupplyDate;
				supply.ModificationDate = DateTime.UtcNow;
				supply.ModifiedBy = _currentUserService.UserId;

				await _supplyRepository.UpdateAsync(supply);

				// Notify the order requester
				if (supply.Order?.RequesterId != null)
				{
					try
					{
						// Get current user details for contact information
						var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId ?? string.Empty);
						var currentUserName = currentUser?.FullNameEN ?? currentUser?.UserName ?? "the administrator";
						var currentUserEmail = currentUser?.Email;
						var currentUserPhone = currentUser?.PhoneNumber;

						// Build contact information string
						var contactParts = new List<string>();
						if (!string.IsNullOrEmpty(currentUserEmail))
							contactParts.Add($"email: {currentUserEmail}");
						if (!string.IsNullOrEmpty(currentUserPhone))
							contactParts.Add($"phone: {currentUserPhone}");

						var contactInfo = contactParts.Any()
							? $"If this date is not suitable, please contact {currentUserName} ({string.Join(" or ", contactParts)}) to arrange an alternative."
							: $"If this date is not suitable, please contact {currentUserName} to arrange an alternative.";

						await _notificationHelperService.SendNotificationAsync(
							title: "Supply Pickup Date Confirmed",
							message: $"The supply pickup date for Order #{supply.Order.RequestNo} has been confirmed to {inputDto.SupplyDate:yyyy-MM-dd}. {contactInfo}",
							entityType: "Supply",
							entityId: supply.Id,
							userIds: new List<string> { supply.Order.RequesterId },
							senderId: _currentUserService.UserId
						);
					}
					catch (Exception ex)
					{
						// Log error but don't fail the operation - notifications are non-critical
						_logger.LogError(ex, "Error sending notification to requester. SupplyId: {SupplyId}, RequesterId: {RequesterId}",
							supply.Id, supply.Order.RequesterId);
					}
				}

				return APIOperationResponse<bool>.Success(true, "Supply pickup date confirmed successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error confirming supply pickup date. OrderId: {OrderId}, User: {UserId}",
					orderId, _currentUserService.UserId);
				return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
			}
		}
	}
}
