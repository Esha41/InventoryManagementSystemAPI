using Microsoft.Extensions.Logging;
using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Entities;
using Ettad.Module.lookup.Dtos;
using Ettad.Lookups.Services.Contracts;

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
    public class DepartmentController : LookupController<Department, DepartmentDto>
    {
        public DepartmentController(ILookupService<Department, DepartmentDto> iLookupService, ILogger<LookupController<Department, DepartmentDto>> logger)
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
    public class PropellantController : LookupController<Propellant, PropellantDto>
    {
        public PropellantController(ILookupService<Propellant, PropellantDto> iLookupService, ILogger<LookupController<Propellant, PropellantDto>> logger)
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
    public class UnitController : LookupController<Unit, UnitDto>
    {
        public UnitController(ILookupService<Unit, UnitDto> iLookupService, ILogger<LookupController<Unit, UnitDto>> logger)
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
    public class ProjectailMaterialController : LookupController<ProjectailMaterial, ProjectailMaterialDto>
    {
        public ProjectailMaterialController(ILookupService<ProjectailMaterial, ProjectailMaterialDto> iLookupService, ILogger<LookupController<ProjectailMaterial, ProjectailMaterialDto>> logger)
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
    public class NatureOptionController : LookupController<NatureOption, NatureOptionDto>
    {
        public NatureOptionController(ILookupService<NatureOption, NatureOptionDto> iLookupService, ILogger<LookupController<NatureOption, NatureOptionDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Nsn

    [CheckAuthorize(
        "Permissions.Nsns.Page",
        "Permissions.Nsns.View",
        "Permissions.Nsns.Create",
        "Permissions.Nsns.Edit",
        "Permissions.Nsns.Delete"
    )]
    public class NsnController : LookupController<Nsn, NsnDto>
    {
        public NsnController(ILookupService<Nsn, NsnDto> iLookupService, ILogger<LookupController<Nsn, NsnDto>> logger)
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
    public class PrimaryPurposController : LookupController<PrimaryPurpos, PrimaryPurposDto>
    {
        public PrimaryPurposController(ILookupService<PrimaryPurpos, PrimaryPurposDto> iLookupService, ILogger<LookupController<PrimaryPurpos, PrimaryPurposDto>> logger)
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
    public class ManufacturerController : LookupController<Manufacturer, ManufacturerDto>
    {
        public ManufacturerController(ILookupService<Manufacturer, ManufacturerDto> iLookupService, ILogger<LookupController<Manufacturer, ManufacturerDto>> logger)
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
    public class HccController : LookupController<Hcc, HccDto>
    {
        public HccController(ILookupService<Hcc, HccDto> iLookupService, ILogger<LookupController<Hcc, HccDto>> logger)
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
    public class DepotController : LookupController<Depot, DepotDto>
    {
        public DepotController(ILookupService<Depot, DepotDto> iLookupService, ILogger<LookupController<Depot, DepotDto>> logger)
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
    public class HazardDivisionController : LookupController<HazardDivision, HazardDivisionDto>
    {
        public HazardDivisionController(ILookupService<HazardDivision, HazardDivisionDto> iLookupService, ILogger<LookupController<HazardDivision, HazardDivisionDto>> logger)
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
    public class CountryController : LookupController<Country, CountryDto>
    {
        public CountryController(ILookupService<Country, CountryDto> iLookupService, ILogger<LookupController<Country, CountryDto>> logger)
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
    public class CaseTypeController : LookupController<CaseType, CaseTypeDto>
    {
        public CaseTypeController(ILookupService<CaseType, CaseTypeDto> iLookupService, ILogger<LookupController<CaseType, CaseTypeDto>> logger)
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
    public class CompatibilityController : LookupController<Compatibility, CompatibilityDto>
    {
        public CompatibilityController(ILookupService<Compatibility, CompatibilityDto> iLookupService, ILogger<LookupController<Compatibility, CompatibilityDto>> logger)
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
    public class ColorController : LookupController<Color, ColorDto>
    {
        public ColorController(ILookupService<Color, ColorDto> iLookupService, ILogger<LookupController<Color, ColorDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion

    #region Supplier

    [CheckAuthorize(
        "Permissions.Suppliers.Page",
        "Permissions.Suppliers.View",
        "Permissions.Suppliers.Create",
        "Permissions.Suppliers.Edit",
        "Permissions.Suppliers.Delete"
    )]
    public class SupplierController : LookupController<Supplier, SupplierDto>
    {
        public SupplierController(ILookupService<Supplier, SupplierDto> iLookupService, ILogger<LookupController<Supplier, SupplierDto>> logger)
            : base(iLookupService, logger) { }
    }

    #endregion
}