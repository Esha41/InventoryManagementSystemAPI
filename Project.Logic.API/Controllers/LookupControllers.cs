using Ettad.CrossCutting.Comman.Models;
using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Module.lookup.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Ettad.Application.Common.Interfaces;
using Ettad.ResponseHandler.Models;
using System.Collections.Generic;
using Ettad.Module.lookup.Interfaces;

namespace Ettad.Lookups.Domain.API.Controllers
{
    #region Department

    [CheckAuthorize(
        "Permissions.Departments.Page",
        "Permissions.Departments.View",
        "Permissions.Departments.Create",
        "Permissions.Departments.Edit",
        "Permissions.Departments.Delete"
    )]
    public class DepartmentController : LookupController<Department, CreateUpdateDepartmentDto>
    {
        public DepartmentController(ILookupService<Department, CreateUpdateDepartmentDto> iLookupService, ILogger<LookupController<Department, CreateUpdateDepartmentDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Propellant

    [CheckAuthorize(
        "Permissions.Propellants.Page",
        "Permissions.Propellants.View",
        "Permissions.Propellants.Create",
        "Permissions.Propellants.Edit",
        "Permissions.Propellants.Delete"
    )]
    public class PropellantController : LookupController<Propellant, CreateUpdatePropellantDto>
    {
        public PropellantController(ILookupService<Propellant, CreateUpdatePropellantDto> iLookupService, ILogger<LookupController<Propellant, CreateUpdatePropellantDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Unit

    [CheckAuthorize(
        "Permissions.Units.Page",
        "Permissions.Units.View",
        "Permissions.Units.Create",
        "Permissions.Units.Edit",
        "Permissions.Units.Delete"
    )]
    public class UnitController : LookupController<Unit, CreateUpdateUnitDto>
    {
        private readonly IUnitLookupService _unitLookupService;

        public UnitController(
            IUnitLookupService unitLookupService,
            ILogger<LookupController<Unit, CreateUpdateUnitDto>> logger)
            : base(unitLookupService, logger)
        {
            _unitLookupService = unitLookupService;
        }

        /// <summary>
        /// Get Units filtered by ItemType
        /// </summary>
        [HttpGet("byItemType")]
        public async Task<IActionResult> GetByItemType([FromQuery] ItemType? itemType)
        {
            _logger?.LogInformation("HTTP GET request for Units filtered by ItemType {ItemType}", itemType);
            var result = await _unitLookupService.GetByItemTypeAsync(itemType);
            return ProcessResponse(result);
        }
    }

    #endregion

    #region ProjectailMaterial

    [CheckAuthorize(
        "Permissions.ProjectailMaterials.Page",
        "Permissions.ProjectailMaterials.View",
        "Permissions.ProjectailMaterials.Create",
        "Permissions.ProjectailMaterials.Edit",
        "Permissions.ProjectailMaterials.Delete"
    )]
    public class ProjectailMaterialController : LookupController<ProjectailMaterial, CreateUpdateProjectailMaterialDto>
    {
        public ProjectailMaterialController(ILookupService<ProjectailMaterial, CreateUpdateProjectailMaterialDto> iLookupService, ILogger<LookupController<ProjectailMaterial, CreateUpdateProjectailMaterialDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region NatureOption

    [CheckAuthorize(
        "Permissions.NatureOptions.Page",
        "Permissions.NatureOptions.View",
        "Permissions.NatureOptions.Create",
        "Permissions.NatureOptions.Edit",
        "Permissions.NatureOptions.Delete"
    )]
    public class NatureOptionController : LookupController<NatureOption, CreateUpdateNatureOptionDto>
    {
        public NatureOptionController(ILookupService<NatureOption, CreateUpdateNatureOptionDto> iLookupService, ILogger<LookupController<NatureOption, CreateUpdateNatureOptionDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region PrimaryPurpos

    [CheckAuthorize(
        "Permissions.PrimaryPurposes.Page",
        "Permissions.PrimaryPurposes.View",
        "Permissions.PrimaryPurposes.Create",
        "Permissions.PrimaryPurposes.Edit",
        "Permissions.PrimaryPurposes.Delete"
    )]
    public class PrimaryPurposController : LookupController<PrimaryPurpos, CreateUpdatePrimaryPurposDto>
    {
        public PrimaryPurposController(ILookupService<PrimaryPurpos, CreateUpdatePrimaryPurposDto> iLookupService, ILogger<LookupController<PrimaryPurpos, CreateUpdatePrimaryPurposDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Manufacturer

    [CheckAuthorize(
        "Permissions.Manufacturers.Page",
        "Permissions.Manufacturers.View",
        "Permissions.Manufacturers.Create",
        "Permissions.Manufacturers.Edit",
        "Permissions.Manufacturers.Delete"
    )]
    public class ManufacturerController : LookupController<Manufacturer, CreateUpdateManufacturerDto>
    {
        public ManufacturerController(ILookupService<Manufacturer, CreateUpdateManufacturerDto> iLookupService, ILogger<LookupController<Manufacturer, CreateUpdateManufacturerDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Depot

    [CheckAuthorize(
        "Permissions.Depots.Page",
        "Permissions.Depots.View",
        "Permissions.Depots.ViewAll",
        "Permissions.Depots.Create",
        "Permissions.Depots.Edit",
        "Permissions.Depots.Delete"
    )]
    public class DepotController : LookupController<Depot, CreateUpdateDepotDto>
    {
        private readonly IDepotService _depotService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserDepotService _userDepotService;

        public DepotController(
            IDepotService depotService, 
            ICurrentUserService currentUserService,
            IUserDepotService userDepotService,
            ILogger<LookupController<Depot, CreateUpdateDepotDto>> logger)
            : base(depotService, logger)
        {
            _depotService = depotService;
            _currentUserService = currentUserService;
            _userDepotService = userDepotService;
        }

        /// <summary>
        /// Paged depot list (same permission and visibility rules as GET /Lookup/Depot).
        /// </summary>
        [HttpPost("paginated")]
        public async Task<IActionResult> GetDepotsPaginated([FromBody] PagedListRequest request)
        {
            _logger?.LogInformation("HTTP POST paginated depot list. Page {Page}, PageSize {PageSize}", request?.Page, request?.PageSize);
            var result = await _depotService.GetDepotsPaginatedAsync(request);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get users assigned to a depot.
        /// </summary>
        [HttpGet("{depotId}/users")]
        public async Task<IActionResult> GetDepotUsers(int depotId)
        {
            _logger?.LogInformation("HTTP GET request for users assigned to depot {DepotId}", depotId);
            var result = await _userDepotService.GetUsersByDepotIdAsync(depotId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Set user assignments for a depot. Replaces existing assignments.
        /// </summary>
        [HttpPut("{depotId}/users")]
        public async Task<IActionResult> SetDepotUsers(int depotId, [FromBody] SetDepotUsersRequest request)
        {
            _logger?.LogInformation("HTTP PUT request to set users for depot {DepotId}", depotId);
            if (request == null)
            {
                return BadRequest("Request body is required.");
            }
            var result = await _userDepotService.SetDepotUserAssignmentsAsync(depotId, request.UserIds ?? new List<string>());
            return ProcessResponse(result);
        }

        /// <summary>
        /// Override Post to restrict depot creation to SuperAdmin only
        /// </summary>
        [HttpPost]
        public override async Task<IActionResult> post([FromBody] CreateUpdateDepotDto item)
        {
            if (!_currentUserService.IsSuperAdmin)
            {
                _logger?.LogWarning("Non-SuperAdmin user attempted to create depot");
                return Forbid();
            }

            return await base.post(item);
        }

        /// <summary>
        /// Override Put - allows all authorized users to update depots
        /// </summary>
        [HttpPut("{id}")]
        public override async Task<ActionResult> Put(int id, [FromBody] CreateUpdateDepotDto item)
        {
            return await base.Put(id, item);
        }

        /// <summary>
        /// Override Delete - allows all authorized users to delete depots (with inventory check)
        /// </summary>
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id, [FromBody] CreateUpdateDepotDto item)
        {
            _logger?.LogInformation("HTTP DELETE request to delete Depot with Id {Id}", id);

            var result = await _depotService.DeleteDepotAsync(id, item);

            if (!result.Succeeded)
            {
                _logger?.LogWarning("Failed to delete Depot with Id {Id}: {Message}", id, result.Message);
            }

            return ProcessResponse(result);
        }
    }

    #endregion

    #region HazardDivision

    [CheckAuthorize(
        "Permissions.HazardDivisions.Page",
        "Permissions.HazardDivisions.View",
        "Permissions.HazardDivisions.Create",
        "Permissions.HazardDivisions.Edit",
        "Permissions.HazardDivisions.Delete"
    )]
    public class HazardDivisionController : LookupController<HazardDivision, CreateUpdateHazardDivisionDto>
    {
        public HazardDivisionController(ILookupService<HazardDivision, CreateUpdateHazardDivisionDto> iLookupService, ILogger<LookupController<HazardDivision, CreateUpdateHazardDivisionDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Country

    [CheckAuthorize(
        "Permissions.Countries.Page",
        "Permissions.Countries.View",
        "Permissions.Countries.Create",
        "Permissions.Countries.Edit",
        "Permissions.Countries.Delete"
    )]
    public class CountryController : LookupController<Country, CreateUpdateCountryDto>
    {
        public CountryController(ILookupService<Country, CreateUpdateCountryDto> iLookupService, ILogger<LookupController<Country, CreateUpdateCountryDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region CaseType

    [CheckAuthorize(
        "Permissions.CaseTypes.Page",
        "Permissions.CaseTypes.View",
        "Permissions.CaseTypes.Create",
        "Permissions.CaseTypes.Edit",
        "Permissions.CaseTypes.Delete"
    )]
    public class CaseTypeController : LookupController<CaseType, CreateUpdateCaseTypeDto>
    {
        public CaseTypeController(ILookupService<CaseType, CreateUpdateCaseTypeDto> iLookupService, ILogger<LookupController<CaseType, CreateUpdateCaseTypeDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Compatibility

    [CheckAuthorize(
        "Permissions.Compatibilities.Page",
        "Permissions.Compatibilities.View",
        "Permissions.Compatibilities.Create",
        "Permissions.Compatibilities.Edit",
        "Permissions.Compatibilities.Delete"
    )]
    public class CompatibilityController : LookupController<Compatibility, CreateUpdateCompatibilityDto>
    {
        public CompatibilityController(ILookupService<Compatibility, CreateUpdateCompatibilityDto> iLookupService, ILogger<LookupController<Compatibility, CreateUpdateCompatibilityDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Color

    [CheckAuthorize(
        "Permissions.Colors.Page",
        "Permissions.Colors.View",
        "Permissions.Colors.Create",
        "Permissions.Colors.Edit",
        "Permissions.Colors.Delete"
    )]
    public class ColorController : LookupController<Color, CreateUpdateColorDto>
    {
        public ColorController(ILookupService<Color, CreateUpdateColorDto> iLookupService, ILogger<LookupController<Color, CreateUpdateColorDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Supplier

    [CheckAuthorize(
        "Permissions.Supplier.Page",
        "Permissions.Supplier.View",
        "Permissions.Supplier.Create",
        "Permissions.Supplier.Edit",
        "Permissions.Supplier.Delete"
    )]
    public class SupplierController : LookupController<Supplier, CreateUpdateSupplierDto>
    {
        public SupplierController(ILookupService<Supplier, CreateUpdateSupplierDto> iLookupService, ILogger<LookupController<Supplier, CreateUpdateSupplierDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Rank

    [CheckAuthorize(
        "Permissions.Rank.Page",
        "Permissions.Rank.View",
        "Permissions.Rank.Create",
        "Permissions.Rank.Edit",
        "Permissions.Rank.Delete"
    )]
    public class RankController : LookupController<Rank, CreateUpdateRankDto>
    {
        public RankController(ILookupService<Rank, CreateUpdateRankDto> iLookupService, ILogger<LookupController<Rank, CreateUpdateRankDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

   // #region Workflow
   // [CheckAuthorize(
   //    "Permissions.WorkFlowType.Page",
   //    "Permissions.WorkFlowType.View",
   //    "Permissions.WorkFlowType.Create",
   //    "Permissions.WorkFlowType.Edit",
   //    "Permissions.WorkFlowType.Delete"
   //)]
   // public class WorkFlowTypeController : LookupController<WorkFlowType, CreateUpdateWorkFlowTypeDto>
   // {
   //     public WorkFlowTypeController(ILookupService<WorkFlowType, CreateUpdateWorkFlowTypeDto> iLookupService, ILogger<LookupController<WorkFlowType, CreateUpdateWorkFlowTypeDto>> logger)
   //         : base(iLookupService, logger) { }
   // }
   // #endregion

    #region Classification

    [CheckAuthorize(
        "Permissions.Classifications.Page",
        "Permissions.Classifications.View",
        "Permissions.Classifications.Create",
        "Permissions.Classifications.Edit",
        "Permissions.Classifications.Delete"
    )]
    public class ClassificationController : LookupController<Classification, CreateUpdateClassificationDto>
    {
        public ClassificationController(ILookupService<Classification, CreateUpdateClassificationDto> iLookupService, ILogger<LookupController<Classification, CreateUpdateClassificationDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region ItemType

    [CheckAuthorize(
        "Permissions.ItemTypes.Page",
        "Permissions.ItemTypes.View",
        "Permissions.ItemTypes.Create",
        "Permissions.ItemTypes.Edit",
        "Permissions.ItemTypes.Delete"
    )]
    public class ItemTypeController : LookupController<ItemTypeLookup, CreateUpdateItemTypeLookupDto>
    {
        public ItemTypeController(ILookupService<ItemTypeLookup, CreateUpdateItemTypeLookupDto> iLookupService, ILogger<LookupController<ItemTypeLookup, CreateUpdateItemTypeLookupDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Caliber

    [CheckAuthorize(
        "Permissions.Calibers.Page",
        "Permissions.Calibers.View",
        "Permissions.Calibers.Create",
        "Permissions.Calibers.Edit",
        "Permissions.Calibers.Delete"
    )]
    public class CaliberController : LookupController<Caliber, CreateUpdateCaliberDto>
    {
        private readonly ICaliberLookupService _caliberLookupService;

        public CaliberController(
            ICaliberLookupService caliberLookupService,
            ILogger<LookupController<Caliber, CreateUpdateCaliberDto>> logger)
            : base(caliberLookupService, logger)
        {
            _caliberLookupService = caliberLookupService;
        }

        /// <summary>
        /// Get calibers filtered by ItemType (Weapon vs Ammunition).
        /// </summary>
        [HttpGet("byItemType")]
        public async Task<IActionResult> GetByItemType([FromQuery] ItemType? itemType)
        {
            _logger?.LogInformation("HTTP GET request for Calibers filtered by ItemType {ItemType}", itemType);
            var result = await _caliberLookupService.GetByItemTypeAsync(itemType);
            return ProcessResponse(result);
        }
    }

    #endregion
}
