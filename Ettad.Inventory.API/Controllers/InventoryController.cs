using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Inventories;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ApiControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly IExcelExportService _excelExportService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(
            IInventoryService inventoryService,
            IExcelExportService excelExportService,
            ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _excelExportService = excelExportService;
            _logger = logger;
        }

        /// <summary>
        /// Get inventory by ID with all details and navigation properties
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _inventoryService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all inventories with details and navigation properties
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _inventoryService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new inventory with details
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Inventory.Create")]
        public async Task<IActionResult> Create([FromBody] CreateInventoryDto dto)
        {
            var result = await _inventoryService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update an existing inventory and its details
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateInventoryDto dto)
        {
            var result = await _inventoryService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Soft delete an inventory
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Inventory.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _inventoryService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all lots for a specific item with usage tracking
        /// </summary>
        [HttpGet("item/{itemId}/lots")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetLotsByItemId(long itemId)
        {
            var result = await _inventoryService.GetLotsByItemIdAsync(itemId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get available lots for a specific item and quantity (excludes expired and empty lots)
        /// </summary>
        [HttpGet("item/{itemId}/available-lots")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetAvailableLotsForQuantity(long itemId, [FromQuery] long quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            var result = await _inventoryService.GetAvailableLotsForQuantityAsync(itemId, quantity);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get detailed information about a specific lot using the lot number
        /// </summary>
        [HttpGet("lot/{lotNumber}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetLotByNumber(int lotNumber)
        {
            var result = await _inventoryService.GetLotByNumberAsync(lotNumber);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get aggregated inventory summary for all items
        /// </summary>
        [HttpGet("items/summary")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetInventorySummaryForAllItems()
        {
            var result = await _inventoryService.GetInventorySummaryForAllItemsAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get aggregated inventory summary for a specific item (total quantity, used, reserved, remaining across all lots)
        /// </summary>
        [HttpGet("item/{itemId}/summary")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetItemInventorySummary(long itemId)
        {
            var result = await _inventoryService.GetItemInventorySummaryAsync(itemId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Toggle the ReadyForIssue status of an inventory detail
        /// </summary>
        [HttpPost("detail/{inventoryDetailId}/toggle-ready-for-issue")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Inventory.Edit")]
        public async Task<IActionResult> ToggleReadyForIssue(long inventoryDetailId)
        {
            var result = await _inventoryService.ToggleReadyForIssueAsync(inventoryDetailId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Export inventory summary to Excel
        /// </summary>
        /// <param name="itemType">Optional filter by item type</param>
        /// <returns>Excel file with inventory data</returns>
        [HttpGet("export")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> ExportToExcel([FromQuery] ItemType? itemType = null)
        {
            try
            {
                _logger.LogInformation("Starting inventory export. ItemType filter: {ItemType}", itemType?.ToString() ?? "All");

                // Get inventory summary data
                var summaryResult = await _inventoryService.GetInventorySummaryForAllItemsAsync();
                
                if (!summaryResult.Succeeded || summaryResult.Data == null)
                {
                    _logger.LogWarning("Failed to retrieve inventory data for export");
                    return BadRequest(new { message = "Failed to retrieve inventory data", errors = summaryResult.Errors });
                }

                var items = summaryResult.Data.AsEnumerable();

                // Apply filter if provided
                if (itemType.HasValue)
                {
                    items = items.Where(x => x.ItemType == itemType.Value);
                    _logger.LogInformation("Filtered inventory by type: {ItemType}. Count: {Count}", itemType.Value, items.Count());
                }

                var itemsList = items.ToList();
                
                if (!itemsList.Any())
                {
                    _logger.LogWarning("No inventory items found for export");
                    return BadRequest(new { message = "No inventory items found to export" });
                }

                // Define column mappings
                var columnMappings = GetInventoryExportColumnMappings();

                // Generate Excel file
                _logger.LogInformation("Generating Excel file for {Count} inventory items", itemsList.Count);
                var excelData = _excelExportService.ExportToExcel(
                    itemsList,
                    "Inventory Summary",
                    columnMappings
                );

                // Generate filename with timestamp and optional type filter
                var typeFilter = itemType.HasValue ? $"_{itemType.Value}" : "";
                var fileName = $"Inventory_Summary{typeFilter}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";

                _logger.LogInformation("Excel export completed successfully. File: {FileName}, Size: {Size} bytes", fileName, excelData.Length);

                // Return file
                return File(
                    excelData,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Excel export for inventory");
                return StatusCode(500, new { message = "An error occurred while generating the Excel file", error = ex.Message });
            }
        }

        /// <summary>
        /// Get column mappings for inventory export
        /// </summary>
        private Dictionary<string, Func<ItemInventorySummaryDto, object>> GetInventoryExportColumnMappings()
        {
            return new Dictionary<string, Func<ItemInventorySummaryDto, object>>
            {
                { "Item Name", item => item.ItemName ?? "" },
                { "Item No", item => item.ItemNo ?? "" },
                { "Type", item => GetItemTypeName(item.ItemType) },
                { "NSN", item => item.Nsn ?? "" },
                { "Part No", item => item.PartNo ?? "" },
                { "Total Quantity", item => item.TotalQuantity },
                { "Used Quantity", item => item.UsedQuantity },
                { "Reserved Quantity", item => item.ReservedQuantityByOrdersOnProcessing },
                { "Remaining Quantity", item => item.RemainingQuantity },
                { "Total Lots", item => item.TotalLots }
            };
        }

        private string GetItemTypeName(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.Ammunition => "Ammunition",
                ItemType.Weapon => "Weapon",
                ItemType.Explosive => "Explosive",
                ItemType.Accessory => "Accessory",
                _ => "Unknown"
            };
        }
    }
}

