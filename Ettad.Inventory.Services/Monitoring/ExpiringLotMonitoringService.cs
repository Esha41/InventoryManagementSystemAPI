using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Data.Entities;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Enums;
using AutoMapper;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.Data.Interfaces.Repositories;

namespace Ettad.Inventory.Service.Monitoring
{
    public class ExpiringLotMonitoringService : IExpiringLotMonitoringService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExpiringLotMonitoringService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailsRepository;
        private readonly ICrossCuttingRepository<Supply> _supplyRepository;

        public ExpiringLotMonitoringService(
            ApplicationDbContext context,
            ILogger<ExpiringLotMonitoringService> logger,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper,
            ICrossCuttingRepository<SupplyDetail> supplyDetailsRepository,
            ICrossCuttingRepository<Supply> supplyRepository)
        {
            _context = context;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
            _supplyDetailsRepository = supplyDetailsRepository;
            _supplyRepository = supplyRepository;
        }

        public async Task<APIOperationResponse<int>> GetExpiringLotsCountAsync(long? depotId = null)
        {
            _logger.LogInformation("Getting expiring lots count (next 30 days). DepotId: {DepotId}", depotId?.ToString() ?? "All");

            try
            {
                var today = _dateTimeProvider.Now.Date;
                var thirtyDaysFromNow = today.AddDays(30);

                var query = _context.InventoryDetails
                    .Include(id => id.Inventory)
                        .ThenInclude(inv => inv.Depo)
                    .Include(id => id.Item)
                    .Include(id => id.Supplier)
                    .Include(id => id.Manufacturer)
                    .Where(id =>
                        id.ExpiryDate.HasValue &&
                        id.ExpiryDate.Value.Date >= today &&
                        id.ExpiryDate.Value.Date <= thirtyDaysFromNow &&
                        !id.Inventory.IsDeleted);

                if (depotId.HasValue)
                    query = query.Where(id => id.Inventory.DepoId == depotId.Value);

                var expiringLots = await query.AsNoTracking().ToListAsync();

                // Filter out empty lots by calculating remaining quantity
                var expiringLotsWithQuantity = new List<InventoryDetail>();

                foreach (var lot in expiringLots)
                {
                    var remainingQuantity = await CalculateRemainingQuantityAsync(lot);
                    if (remainingQuantity > 0)
                    {
                        expiringLotsWithQuantity.Add(lot);
                    }
                }

                _logger.LogInformation($"Found {expiringLotsWithQuantity.Count} lots expiring in the next 30 days.");

                return APIOperationResponse<int>.Success(expiringLotsWithQuantity.Count);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting expiring lots count.");
                return APIOperationResponse<int>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ExpiringLotDto>>> GetExpiringLotsAsync()
        {
            _logger.LogInformation("Getting expiring lots with details (next 30 days)");

            try
            {
                var today = _dateTimeProvider.Now.Date;
                var thirtyDaysFromNow = today.AddDays(30);

                var expiringLots = await _context.InventoryDetails
                    .Include(id => id.Inventory)
                        .ThenInclude(inv => inv.Depo)
                    .Include(id => id.Item)
                    .Include(id => id.Supplier)
                    .Include(id => id.Manufacturer)
                    .Where(id => 
                        id.ExpiryDate.HasValue &&
                        id.ExpiryDate.Value.Date >= today &&
                        id.ExpiryDate.Value.Date <= thirtyDaysFromNow &&
                        !id.Inventory.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation($"Found {expiringLots.Count} lots with expiry dates in the next 30 days.");

                var expiringLotDtos = new List<ExpiringLotDto>();

                foreach (var lot in expiringLots)
                {
                    var remainingQuantity = await CalculateRemainingQuantityAsync(lot);
                    
                    // Only include lots with remaining quantity > 0
                    if (remainingQuantity > 0)
                    {
                        var daysUntilExpiry = lot.ExpiryDate.HasValue
                            ? (int)(lot.ExpiryDate.Value.Date - today).TotalDays
                            : (int?)null;

                        var lotDetail = _mapper.Map<LotDetailDto>(lot);
                        
                        expiringLotDtos.Add(new ExpiringLotDto
                        {
                            InventoryDetailId = lot.Id,
                            ItemId = lot.ItemId,
                            ItemName = lot.Item?.Name ?? string.Empty,
                            ItemNo = lot.Item?.ItemNo,
                            Lot = lot.Lot,
                            BatchNo = lot.BatchNo,
                            ExpiryDate = lot.ExpiryDate,
                            DaysUntilExpiry = daysUntilExpiry,
                            RemainingQuantity = remainingQuantity,
                            Depot = lotDetail.Depot,
                            Supplier = lotDetail.Supplier,
                            Manufacturer = lotDetail.Manufacturer
                        });
                    }
                }

                // Sort by expiry date (earliest first)
                expiringLotDtos = expiringLotDtos
                    .OrderBy(l => l.ExpiryDate)
                    .ThenBy(l => l.Lot)
                    .ToList();

                _logger.LogInformation($"Found {expiringLotDtos.Count} lots expiring in the next 30 days with remaining quantity.");

                return APIOperationResponse<List<ExpiringLotDto>>.Success(expiringLotDtos);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting expiring lots.");
                return APIOperationResponse<List<ExpiringLotDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<long> CalculateRemainingQuantityAsync(InventoryDetail lot)
        {
            // Get supply details for this lot
            var allSupplyDetails = await _supplyDetailsRepository.FindAsync(
                sd => sd.ItemId == lot.ItemId && sd.Lot == lot.Lot && !sd.IsDeleted
            );

            // Get all supplies to check submission status
            var supplyIds = allSupplyDetails.Select(sd => sd.SupplyId).Distinct().ToList();
            var supplies = supplyIds.Any()
                ? await _supplyRepository.FindAsync(s => supplyIds.Contains(s.Id) && !s.IsDeleted)
                : new List<Supply>();

            var supplyStatusMap = supplies.ToDictionary(s => s.Id, s => s.SubmissionStatus);

            // Calculate used quantity (submitted supplies)
            var usedQuantity = allSupplyDetails
                .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) && 
                           supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Submitted)
                .Sum(sd => sd.Quantity);

            // Calculate reserved quantity (draft supplies)
            var reservedQuantity = allSupplyDetails
                .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) && 
                           supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Draft)
                .Sum(sd => sd.Quantity);

            // Calculate remaining quantity
            var remainingQuantity = lot.ItemQuantity - usedQuantity - reservedQuantity;

            return Math.Max(0, remainingQuantity);
        }
    }
}
