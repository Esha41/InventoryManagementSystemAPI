using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Inventories;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.EntityFramework.DataBaseContext;

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
        private readonly ApplicationDbContext _context;

        public InventoryController(
            IInventoryService inventoryService,
            IExcelExportService excelExportService,
            ILogger<InventoryController> logger,
            ApplicationDbContext context)
        {
            _inventoryService = inventoryService;
            _excelExportService = excelExportService;
            _logger = logger;
            _context = context;
            
            // Set EPPlus license context
            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
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
        /// Import inventory from Excel file
        /// </summary>
        [HttpPost("Import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.Create")]
        public async Task<IActionResult> Import(IFormFile file, [FromForm] long depotId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "File is required" });
            }

            if (depotId <= 0)
            {
                return BadRequest(new { message = "Valid depot ID is required" });
            }

            _logger.LogInformation("Inventory import request received. DepotId: {DepotId}, FileName: {FileName}, FileSize: {FileSize}",
                depotId, file.FileName, file.Length);

            var result = await _inventoryService.ImportAsync(file, depotId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Preview inventory import from Excel file (validation only, no data saved)
        /// </summary>
        [HttpPost("ImportPreview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.Create")]
        public async Task<IActionResult> ImportPreview(IFormFile file, [FromForm] long depotId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "File is required" });
            }

            if (depotId <= 0)
            {
                return BadRequest(new { message = "Valid depot ID is required" });
            }

            _logger.LogInformation("Inventory import preview request received. DepotId: {DepotId}, FileName: {FileName}, FileSize: {FileSize}",
                depotId, file.FileName, file.Length);

            var result = await _inventoryService.ImportPreviewAsync(file, depotId);
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
                var fileName = $"Inventory_Summary{typeFilter}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

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

        /// <summary>
        /// Generate inventory import template with Excel data validation (dropdowns for lookups)
        /// </summary>
        /// <param name="depotId">Depot ID for the template</param>
        /// <returns>Excel file with data validation dropdowns</returns>
        [HttpGet("template")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [CheckAuthorize("Permissions.Inventory.Create")]
        public async Task<IActionResult> GenerateImportTemplate([FromQuery] long depotId)
        {
            try
            {
                if (depotId <= 0)
                {
                    return BadRequest(new { message = "Valid depot ID is required" });
                }

                _logger.LogInformation("Generating inventory import template. DepotId: {DepotId}", depotId);

                // Load all items (ammunition, weapons, explosives)
                var ammunitions = await _context.Ammunitions
                    .Where(a => !a.IsDeleted)
                    .ToListAsync();
                
                var weapons = await _context.Weapons
                    .Where(w => !w.IsDeleted)
                    .ToListAsync();
                
                var explosives = await _context.Explosives
                    .Where(e => !e.IsDeleted)
                    .ToListAsync();

                // Combine all items and format as "ItemName (ItemNo)"
                var allItems = new List<(string DisplayName, string ItemNo)>();
                foreach (var item in ammunitions.Cast<BaseItem>().Concat(weapons.Cast<BaseItem>()).Concat(explosives.Cast<BaseItem>()))
                {
                    var itemName = item.Name ?? "";
                    var itemNo = item.ItemNo ?? "";
                    if (!string.IsNullOrWhiteSpace(itemName) && !string.IsNullOrWhiteSpace(itemNo))
                    {
                        allItems.Add(($"{itemName} ({itemNo})", itemNo));
                    }
                }
                allItems = allItems.OrderBy(i => i.DisplayName).ToList();

                // Load lookup data
                var suppliers = await _context.Suppliers
                    .Where(s => !s.IsDeleted)
                    .OrderBy(s => s.NameEn ?? s.NameAr)
                    .ToListAsync();

                var manufacturers = await _context.Manufacturers
                    .Where(m => !m.IsDeleted)
                    .OrderBy(m => m.NameEn ?? m.NameAr)
                    .ToListAsync();

                var countries = await _context.Countries
                    .Where(c => !c.IsDeleted)
                    .OrderBy(c => c.NameEn ?? c.NameAr)
                    .ToListAsync();

                // Generate Excel with EPPlus
                using var package = new ExcelPackage();
                
                // Main template sheet
                var templateSheet = package.Workbook.Worksheets.Add("Import Template");
                
                // Headers - Item Name with dropdown, Item ID removed (auto-resolved from ItemNo)
                var headers = new[]
                {
                    "Item Name", "Lot", "Supplier", "Manufacturer", "Country",
                    "Original Quantity", "Batch No", "Expiry Date", "Ready For Issue",
                    "Invoice Number", "Invoice Date", "Received Date", "Notes"
                };

                for (int col = 1; col <= headers.Length; col++)
                {
                    templateSheet.Cells[1, col].Value = headers[col - 1];
                    templateSheet.Cells[1, col].Style.Font.Bold = true;
                    templateSheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    templateSheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    templateSheet.Cells[1, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Sample data row
                templateSheet.Cells[2, 1].Value = allItems.FirstOrDefault().DisplayName ?? ""; // Item Name (dropdown)
                templateSheet.Cells[2, 2].Value = 1; // Lot
                templateSheet.Cells[2, 3].Value = ""; // Supplier (will have dropdown)
                templateSheet.Cells[2, 4].Value = ""; // Manufacturer (will have dropdown)
                templateSheet.Cells[2, 5].Value = ""; // Country (will have dropdown)
                templateSheet.Cells[2, 6].Value = 100; // Original Quantity
                templateSheet.Cells[2, 7].Value = "BATCH001"; // Batch No
                templateSheet.Cells[2, 8].Value = "2025-12-31"; // Expiry Date
                templateSheet.Cells[2, 9].Value = "Yes"; // Ready For Issue
                templateSheet.Cells[2, 10].Value = "INV-001"; // Invoice Number
                templateSheet.Cells[2, 11].Value = "2025-01-01"; // Invoice Date
                templateSheet.Cells[2, 12].Value = "2025-01-02"; // Received Date
                templateSheet.Cells[2, 13].Value = "Sample inventory entry"; // Notes

                // Create hidden lookup sheets
                // Items sheet with display names (ItemName (ItemNo)) in column A, ItemNo in column B
                var itemsSheet = CreateItemsLookupSheet(package, "Items", allItems);
                var suppliersSheet = CreateLookupSheet(package, "Suppliers", suppliers.Select(s => s.NameEn ?? s.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                var manufacturersSheet = CreateLookupSheet(package, "Manufacturers", manufacturers.Select(m => m.NameEn ?? m.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                var countriesSheet = CreateLookupSheet(package, "Countries", countries.Select(c => c.NameEn ?? c.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());

                // Add data validation dropdowns
                // Item Name column (column A = 1) - shows "ItemName (ItemNo)" format
                AddDataValidation(templateSheet, 1, itemsSheet, "Items");
                
                // Supplier column (column C = 3)
                AddDataValidation(templateSheet, 3, suppliersSheet, "Suppliers");
                
                // Manufacturer column (column D = 4)
                AddDataValidation(templateSheet, 4, manufacturersSheet, "Manufacturers");
                
                // Country column (column E = 5)
                AddDataValidation(templateSheet, 5, countriesSheet, "Countries");

                // Ready For Issue column (column I = 9) - Yes/No dropdown
                var readyForIssueValidation = templateSheet.DataValidations.AddListValidation("I2:I10000");
                readyForIssueValidation.Formula.Values.Add("Yes");
                readyForIssueValidation.Formula.Values.Add("No");
                readyForIssueValidation.ShowErrorMessage = true;
                readyForIssueValidation.ErrorTitle = "Invalid Value";
                readyForIssueValidation.Error = "Please select 'Yes' or 'No'";

                // Set column widths
                templateSheet.Column(1).Width = 30; // Item Name
                templateSheet.Column(2).Width = 10; // Lot
                templateSheet.Column(3).Width = 20; // Supplier
                templateSheet.Column(4).Width = 20; // Manufacturer
                templateSheet.Column(5).Width = 20; // Country
                templateSheet.Column(6).Width = 18; // Original Quantity
                templateSheet.Column(7).Width = 15; // Batch No
                templateSheet.Column(8).Width = 15; // Expiry Date
                templateSheet.Column(9).Width = 15; // Ready For Issue
                templateSheet.Column(10).Width = 20; // Invoice Number
                templateSheet.Column(11).Width = 15; // Invoice Date
                templateSheet.Column(12).Width = 15; // Received Date
                templateSheet.Column(13).Width = 30; // Notes

                // Freeze header row
                templateSheet.View.FreezePanes(2, 1);

                // Generate filename
                var fileName = $"Warehouse_Inventory_Import_Template_Depot_{depotId}.xlsx";
                var excelData = package.GetAsByteArray();

                _logger.LogInformation("Inventory import template generated successfully. DepotId: {DepotId}, FileSize: {FileSize} bytes", 
                    depotId, excelData.Length);

                return File(
                    excelData,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating inventory import template. DepotId: {DepotId}", depotId);
                return StatusCode(500, new { message = "An error occurred while generating the template", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a hidden sheet with lookup values
        /// </summary>
        private ExcelWorksheet CreateLookupSheet(ExcelPackage package, string sheetName, List<string> values)
        {
            var lookupSheet = package.Workbook.Worksheets.Add(sheetName);
            lookupSheet.Hidden = eWorkSheetHidden.Hidden;

            for (int i = 0; i < values.Count; i++)
            {
                lookupSheet.Cells[i + 1, 1].Value = values[i];
            }

            return lookupSheet;
        }

        /// <summary>
        /// Create a hidden sheet with items (DisplayName in column A, ItemNo in column B)
        /// </summary>
        private ExcelWorksheet CreateItemsLookupSheet(ExcelPackage package, string sheetName, List<(string DisplayName, string ItemNo)> items)
        {
            var lookupSheet = package.Workbook.Worksheets.Add(sheetName);
            lookupSheet.Hidden = eWorkSheetHidden.Hidden;

            for (int i = 0; i < items.Count; i++)
            {
                lookupSheet.Cells[i + 1, 1].Value = items[i].DisplayName; // "ItemName (ItemNo)"
                lookupSheet.Cells[i + 1, 2].Value = items[i].ItemNo; // ItemNo for reference
            }

            return lookupSheet;
        }

        /// <summary>
        /// Add data validation dropdown to a column using worksheet reference
        /// </summary>
        private void AddDataValidation(ExcelWorksheet worksheet, int column, ExcelWorksheet lookupSheet, string lookupSheetName)
        {
            var columnLetter = GetColumnLetter(column);
            var validationRange = $"{columnLetter}2:{columnLetter}10000";
            
            var validation = worksheet.DataValidations.AddListValidation(validationRange);
            
            // Use worksheet reference formula: 'SheetName'!$A$1:$A$N
            var lastRow = lookupSheet.Dimension?.End.Row ?? 1;
            validation.Formula.ExcelFormula = $"'{lookupSheetName}'!$A$1:$A${lastRow}";
            
            validation.ShowErrorMessage = true;
            validation.ErrorTitle = "Invalid Value";
            validation.Error = $"Please select a value from the {lookupSheetName} list";
            validation.ShowInputMessage = true;
            validation.PromptTitle = "Select Value";
            validation.Prompt = $"Select a {lookupSheetName.ToLower()} from the dropdown list";
        }

        /// <summary>
        /// Convert column number to Excel column letter (1 = A, 2 = B, etc.)
        /// </summary>
        private string GetColumnLetter(int columnNumber)
        {
            string columnLetter = "";
            while (columnNumber > 0)
            {
                columnNumber--;
                columnLetter = (char)('A' + columnNumber % 26) + columnLetter;
                columnNumber /= 26;
            }
            return columnLetter;
        }
    }
}

