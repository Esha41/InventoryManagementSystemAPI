using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ettad.CrossCutting.Comman;
using Ettad.Lookups.Services.Contracts;
using Ettad.ResponseHandler.Models;

namespace Ettad.Lookups.Domain.API.Controllers
{

    [Route("api/Lookup/[controller]")]
    [Produces("application/json")]

    public abstract class LookupController<T, TDto> : ApiControllerBase where T : class, ILookup where TDto : class 
    {
        protected readonly ILookupService<T, TDto> _lookupService;
        protected readonly ILogger<LookupController<T, TDto>> _logger;
        private readonly string? _parentKey;

        public LookupController(ILookupService<T, TDto> lookupService, ILogger<LookupController<T, TDto>> logger, string parentKey)
        {
            _lookupService = lookupService;
            _logger = logger;
            _parentKey = parentKey;
        }

        public LookupController(ILookupService<T, TDto> lookupService)
        {
            _lookupService = lookupService;
            _logger = null!; // This constructor is for backward compatibility, logger will be null
        }

        public LookupController(ILookupService<T, TDto> lookupService, ILogger<LookupController<T, TDto>> logger)
        {
            _lookupService = lookupService;
            _logger = logger;
        }

        /// <summary>
        /// Get all Lookup Items.
        /// </summary>
        /// <param name="lookupName"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<IActionResult> Get(string controller)
        {
            _logger?.LogInformation("HTTP GET request for all {LookupType} lookup items from controller {Controller}", 
                typeof(T).Name, controller);
            
            var result = await _lookupService.GetLookupItems();
            
            if (!result.Succeeded)
            {
                _logger?.LogWarning("Failed to retrieve {LookupType} lookup items: {Message}", 
                    typeof(T).Name, result.Message);
            }
            
            return ProcessResponse(result);
        }

        [HttpPost]
        public virtual async Task<IActionResult> post( [FromBody] TDto item)
        {
            _logger?.LogInformation("HTTP POST request to create new {LookupType} lookup item", typeof(T).Name);
            
            var result = await _lookupService.AddLookupItem(item);
            
            if (!result.Succeeded)
            {
                _logger?.LogWarning("Failed to create {LookupType} lookup item: {Message}", 
                    typeof(T).Name, result.Message);
            }
            
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult> Put(int id, [FromBody] TDto item)
        {
            _logger?.LogInformation("HTTP PUT request to update {LookupType} lookup item with Id {Id}", 
                typeof(T).Name, id);
            
            var result = await _lookupService.UpdateLookupItem(id, item);
            
            if (!result.Succeeded)
            {
                _logger?.LogWarning("Failed to update {LookupType} lookup item with Id {Id}: {Message}", 
                    typeof(T).Name, id, result.Message);
            }
            
            return ProcessResponse(result);
        }

        /// <summary>
        /// Search lookup items by code, name, or nameSE
        /// </summary>
        [HttpGet("search")]
        public virtual async Task<ActionResult> Search([FromQuery] string searchText, [FromQuery] bool includeDeleted = false)
        {
            _logger?.LogInformation("HTTP GET search request for {LookupType} lookup items with text '{SearchText}'", 
                typeof(T).Name, searchText);
            
            var results = await _lookupService.SearchLookupItems(searchText, includeDeleted);
            
            if (!results.Succeeded)
            {
                _logger?.LogWarning("Failed to search {LookupType} lookup items: {Message}", 
                    typeof(T).Name, results.Message);
            }
            
            return ProcessResponse(results);
        }
        /// <summary>
        /// Get Lookup Item by Id.
        /// </summary>
        /// <param name="lookupName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> LookupItem(string controller, int Id)
        {
            _logger?.LogInformation("HTTP GET request for {LookupType} lookup item with Id {Id} from controller {Controller}", 
                typeof(T).Name, Id, controller);
            
            var result = await _lookupService.GetLookupItemById(Id);
            
            if (!result.Succeeded)
            {
                _logger?.LogWarning("Failed to retrieve {LookupType} lookup item with Id {Id}: {Message}", 
                    typeof(T).Name, Id, result.Message);
            }
            
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Lookup Items using IsDeleted Flag.
        /// </summary>
        /// <param name="lookupName"></param>
        /// <param name="IncludeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("LookupItemsWithDeleted/{IncludeDeleted?}")]
        public async Task<IActionResult> LookupItems(string controller, bool? IncludeDeleted = false)
        {
            _logger?.LogInformation("HTTP GET request for {LookupType} lookup items (IncludeDeleted: {IncludeDeleted}) from controller {Controller}", 
                typeof(T).Name, IncludeDeleted, controller);
            
            var result = await _lookupService.GetLookupItems(IncludeDeleted ?? false);
            
            if (!result.Succeeded)
            {
                _logger?.LogWarning("Failed to retrieve {LookupType} lookup items with deleted flag: {Message}", 
                    typeof(T).Name, result.Message);
            }
            
            return ProcessResponse(result);
        }

        [HttpGet]
        [Route("LookupItemsByParentId/{ParentId}/{IncludeDeleted?}")]
        public async Task<IActionResult> LookupItemsByParentId(string controller, int ParentId, bool? IncludeDeleted = false)
        {
            _logger?.LogInformation("HTTP GET request for {LookupType} lookup items by ParentId {ParentId} from controller {Controller}", 
                typeof(T).Name, ParentId, controller);
            
            var result = await _lookupService.GetLookupItemsByParentId(_parentKey ?? "", ParentId, IncludeDeleted ?? false);
            
            if (!result.Succeeded)
            {
                _logger?.LogWarning("Failed to retrieve {LookupType} lookup items by ParentId {ParentId}: {Message}", 
                    typeof(T).Name, ParentId, result.Message);
            }
            
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id, [FromBody] TDto item)
        {
            _logger?.LogInformation("HTTP DELETE request to soft delete {LookupType} lookup item with Id {Id}", 
                typeof(T).Name, id);
            
            var result = await _lookupService.SoftDeleteLookupItem(id, item);
            
            if (!result.Succeeded)
            {
                _logger?.LogWarning("Failed to soft delete {LookupType} lookup item with Id {Id}: {Message}", 
                    typeof(T).Name, id, result.Message);
            }
            
            return ProcessResponse(result);
        }


    }
}