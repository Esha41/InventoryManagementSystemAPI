using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Lookups.Services.Contracts;
using Ettad.Module.lookup.Dtos;
using Microsoft.Extensions.Logging;

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
        public UnitController(ILookupService<Unit, CreateUpdateUnitDto> iLookupService, ILogger<LookupController<Unit, CreateUpdateUnitDto>> logger)
            : base(iLookupService, logger) { }
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

    #region Hcc

    [CheckAuthorize(
        "Permissions.Hccs.Page",
        "Permissions.Hccs.View",
        "Permissions.Hccs.Create",
        "Permissions.Hccs.Edit",
        "Permissions.Hccs.Delete"
    )]
    public class HccController : LookupController<Hcc, CreateUpdateHccDto>
    {
        public HccController(ILookupService<Hcc, CreateUpdateHccDto> iLookupService, ILogger<LookupController<Hcc, CreateUpdateHccDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Depot

    [CheckAuthorize(
        "Permissions.Depots.Page",
        "Permissions.Depots.View",
        "Permissions.Depots.Create",
        "Permissions.Depots.Edit",
        "Permissions.Depots.Delete"
    )]
    public class DepotController : LookupController<Depot, CreateUpdateDepotDto>
    {
        public DepotController(ILookupService<Depot, CreateUpdateDepotDto> iLookupService, ILogger<LookupController<Depot, CreateUpdateDepotDto>> logger)
            : base(iLookupService, logger) { }
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

    #region Workflow
    [CheckAuthorize(
       "Permissions.WorkFlowType.Page",
       "Permissions.WorkFlowType.View",
       "Permissions.WorkFlowType.Create",
       "Permissions.WorkFlowType.Edit",
       "Permissions.WorkFlowType.Delete"
   )]
    public class WorkFlowTypeController : LookupController<WorkFlowType, CreateUpdateWorkFlowTypeDto>
    {
        public WorkFlowTypeController(ILookupService<WorkFlowType, CreateUpdateWorkFlowTypeDto> iLookupService, ILogger<LookupController<WorkFlowType, CreateUpdateWorkFlowTypeDto>> logger)
            : base(iLookupService, logger) { }
    }
    #endregion
}