using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.CrossCutting.Comman.Models;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using Ettad.Inventory.Service.Inventories.Interfaces;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ApiControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(
            IInventoryService inventoryService,
            ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _inventoryService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _inventoryService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpPost("depot/{depotId}/details/search")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetInventoryDetailsByDepotIdPaginated(long depotId, [FromBody] PagedListRequest request)
        {
            var result = await _inventoryService.GetInventoryDetailsByDepotIdPaginatedAsync(depotId, request);
            return ProcessResponse(result);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Inventory.Create")]
        public async Task<IActionResult> Create([FromForm] CreateInventoryDto dto, [FromForm] List<IFormFile>? files = null)
        {
            var result = await _inventoryService.CreateAsync(dto, files);
            return ProcessResponse(result);
        }

        [HttpPost("Import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.Create")]
        public async Task<IActionResult> Import(IFormFile file, [FromForm] long depotId, [FromQuery] string language = "en")
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is required" });

            if (depotId <= 0)
                return BadRequest(new { message = "Valid depot ID is required" });

            _logger.LogInformation("Inventory import request. DepotId: {DepotId}, File: {FileName}", depotId, file.FileName);

            var result = await _inventoryService.ImportAsync(file, depotId, language);
            return ProcessResponse(result);
        }

        [HttpPost("ImportPreview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.Create")]
        public async Task<IActionResult> ImportPreview(IFormFile file, [FromForm] long depotId, [FromQuery] string language = "en")
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is required" });

            if (depotId <= 0)
                return BadRequest(new { message = "Valid depot ID is required" });

            _logger.LogInformation("Inventory import preview request. DepotId: {DepotId}, File: {FileName}", depotId, file.FileName);

            var result = await _inventoryService.ImportPreviewAsync(file, depotId, language);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.Edit")]
        public async Task<IActionResult> Update(long id, [FromForm] string dto, [FromForm] List<IFormFile>? files = null, [FromForm] long? filesItemId = null)
        {
            if (string.IsNullOrWhiteSpace(dto))
                return BadRequest("dto is required");

            UpdateInventoryDto? parsedDto;
            try
            {
                parsedDto = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateInventoryDto>(dto);
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                return BadRequest($"Invalid JSON for dto: {ex.Message}");
            }

            if (parsedDto == null)
                return BadRequest("Invalid dto payload");

            var result = await _inventoryService.UpdateAsync(id, parsedDto, files, filesItemId);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Inventory.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _inventoryService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet("item/{itemId}/lots")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetLotsByItemId(long itemId, [FromQuery] long? depotId = null)
        {
            var result = await _inventoryService.GetLotsByItemIdAsync(itemId, depotId);
            return ProcessResponse(result);
        }

        [HttpGet("item/{itemId}/available-lots")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetAvailableLotsForQuantity(long itemId, [FromQuery] long quantity, [FromQuery] long? excludeSupplyId = null)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than 0");

            var result = await _inventoryService.GetAvailableLotsForQuantityAsync(itemId, quantity, null, excludeSupplyId);
            return ProcessResponse(result);
        }

        [HttpGet("lot/{lotNumber}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetLotByNumber(string lotNumber)
        {
            var result = await _inventoryService.GetLotByNumberAsync(lotNumber);
            return ProcessResponse(result);
        }

        [HttpGet("items/summary")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetInventorySummaryForAllItems([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _inventoryService.GetInventorySummaryForAllItemsAsync(depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Paginated item-level inventory summary (same depot filters as GET items/summary).
        /// </summary>
        [HttpPost("items/summary/paged")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetInventorySummaryForAllItemsPaged(
            [FromBody] PagedListRequest request,
            [FromQuery] long? depotId = null,
            [FromQuery] List<long>? depotIds = null,
            [FromQuery] ItemType? itemType = null)
        {
            var result = await _inventoryService.GetInventorySummaryForAllItemsPaginatedAsync(request, depotId, depotIds, itemType);
            return ProcessResponse(result);
        }

        [HttpGet("item/{itemId}/summary")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetItemInventorySummary(long itemId)
        {
            var result = await _inventoryService.GetItemInventorySummaryAsync(itemId);
            return ProcessResponse(result);
        }

        [HttpPost("detail/{inventoryDetailId}/toggle-ready-for-issue")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Inventory.Edit")]
        public async Task<IActionResult> ToggleReadyForIssue(long inventoryDetailId)
        {
            var result = await _inventoryService.ToggleReadyForIssueAsync(inventoryDetailId);
            return ProcessResponse(result);
        }

        [HttpGet("export")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> ExportToExcel([FromQuery] ItemType? itemType = null)
        {
            var result = await _inventoryService.ExportToExcelAsync(itemType);
            if (!result.Succeeded)
                return BadRequest(new { message = result.Message });

            var typeFilter = itemType.HasValue ? $"_{itemType.Value}" : "";
            var fileName = $"Inventory_Summary{typeFilter}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";

            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("template")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Inventory.Create")]
        public async Task<IActionResult> GenerateImportTemplate([FromQuery] long depotId, [FromQuery] string language = "en")
        {
            if (depotId <= 0)
                return BadRequest(new { message = "Valid depot ID is required" });

            var result = await _inventoryService.GenerateImportTemplateAsync(depotId, language);
            if (!result.Succeeded)
                return BadRequest(new { message = result.Message });

            var fileName = $"Warehouse_Inventory_Import_Template_Depot_{depotId}.xlsx";
            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
