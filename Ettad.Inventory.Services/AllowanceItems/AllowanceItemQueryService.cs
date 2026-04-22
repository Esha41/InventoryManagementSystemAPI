using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.AllowanceItems.Dtos;
using Ettad.Data.Enums;
using Microsoft.Extensions.Logging;
using Ettad.Data.Interfaces.Repositories;

namespace Ettad.Inventory.Service.AllowanceItems
{
    /// <summary>
    /// Query service for fetching allowance items without permission checks.
    /// Used by DevExpress reports and other services that don't have HttpContext.
    /// </summary>
    public class AllowanceItemQueryService : IAllowanceItemQueryService
    {
        private readonly ICrossCuttingRepository<AllowanceItem> _allowanceItemRepository;
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<Supply> _supplyRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AllowanceItemQueryService> _logger;

        public AllowanceItemQueryService(
            ICrossCuttingRepository<AllowanceItem> allowanceItemRepository,
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<Supply> supplyRepository,
            ICrossCuttingRepository<SupplyDetail> supplyDetailRepository,
            IMapper mapper,
            ILogger<AllowanceItemQueryService> logger)
        {
            _allowanceItemRepository = allowanceItemRepository;
            _orderRepository = orderRepository;
            _supplyRepository = supplyRepository;
            _supplyDetailRepository = supplyDetailRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<AllowanceItemDto>> GetAllAsync()
        {
            try
            {
                // Get all non-deleted items without any department filtering or permission checks
                var allowanceItems = await _allowanceItemRepository.FindAsync(
                    a => !a.IsDeleted,
                    false,
                    nameof(AllowanceItem.Item));

                var dtos = _mapper.Map<List<AllowanceItemDto>>(allowanceItems);
                
                // Populate calculated quantities for each DTO
                foreach (var dto in dtos)
                {
                    var allowanceItem = allowanceItems.FirstOrDefault(a => a.Id == dto.Id);
                    if (allowanceItem != null)
                    {
                        await PopulateCalculatedQuantitiesAsync(dto, allowanceItem);
                    }
                }
                
                _logger.LogInformation("Retrieved {Count} allowance items (no permission filtering)", dtos.Count);
                
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving allowance items in query service");
                throw;
            }
        }

        private async Task PopulateCalculatedQuantitiesAsync(AllowanceItemDto dto, AllowanceItem allowanceItem)
        {
            try
            {
                // Get all orders from this department and year
                var orders = await _orderRepository.FindAsync(
                    o => o.DepartmentId == allowanceItem.DepartmentId &&
                         o.CreationDate.Year == allowanceItem.Year &&
                         !o.IsDeleted);

                var orderIds = orders.Select(o => o.Id).ToList();

                if (!orderIds.Any())
                {
                    dto.UsedQuantityFromAllowance = 0;
                    dto.ReservedQuantityByOrdersOnProcessing = 0;
                    dto.RemainingQuantityFromAllowance = allowanceItem.Quantity;
                    return;
                }

                // Get all supplies for orders that have supplies (only consider orders with supplies)
                var supplies = await _supplyRepository.FindAsync(
                    s => orderIds.Contains(s.OrderId) && !s.IsDeleted);

                // Separate Draft and Submitted supplies
                var draftSupplyIds = supplies
                    .Where(s => s.SubmissionStatus == SupplySubmissionStatus.Draft)
                    .Select(s => s.Id)
                    .ToList();

                var submittedSupplyIds = supplies
                    .Where(s => s.SubmissionStatus == SupplySubmissionStatus.Submitted)
                    .Select(s => s.Id)
                    .ToList();

                // Calculate UsedQuantityFromAllowance from Submitted supplies
                if (submittedSupplyIds.Any())
                {
                    var submittedSupplyDetails = await _supplyDetailRepository.FindAsync(
                        sd => submittedSupplyIds.Contains(sd.SupplyId) &&
                              sd.ItemId == allowanceItem.ItemId &&
                              !sd.IsDeleted);

                    dto.UsedQuantityFromAllowance = (int)submittedSupplyDetails.Sum(sd => sd.Quantity);
                }
                else
                {
                    dto.UsedQuantityFromAllowance = 0;
                }

                // Calculate ReservedQuantityByOrdersUnderProccessing from Draft supplies
                if (draftSupplyIds.Any())
                {
                    var draftSupplyDetails = await _supplyDetailRepository.FindAsync(
                        sd => draftSupplyIds.Contains(sd.SupplyId) &&
                              sd.ItemId == allowanceItem.ItemId &&
                              !sd.IsDeleted);

                    dto.ReservedQuantityByOrdersOnProcessing = (int)draftSupplyDetails.Sum(sd => sd.Quantity);
                }
                else
                {
                    dto.ReservedQuantityByOrdersOnProcessing = 0;
                }

                dto.RemainingQuantityFromAllowance = allowanceItem.Quantity - dto.UsedQuantityFromAllowance - dto.ReservedQuantityByOrdersOnProcessing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating quantities for AllowanceItem. Id: {Id}, ItemId: {ItemId}, DepartmentId: {DepartmentId}, Year: {Year}",
                    allowanceItem.Id, allowanceItem.ItemId, allowanceItem.DepartmentId, allowanceItem.Year);
                
                // Set defaults on error
                dto.UsedQuantityFromAllowance = 0;
                dto.ReservedQuantityByOrdersOnProcessing = 0;
                dto.RemainingQuantityFromAllowance = allowanceItem.Quantity;
            }
        }
    }
}
