//to do

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//using Ettad.Lookups.Domain.API.Controllers;
//using Ettad.CrossCutting.Common.Security;

//namespace Ettad.Lookups.Domain.API.Controllers
//{

//    [CheckAuthorize(
// "Permissions.Organizations.Page",
// "Permissions.Organizations.View",
// "Permissions.Organizations.Create", "Permissions.Organizations.Edit", "Permissions.Organizations.Delete"
//)]
//    public class OrganizationController : LookupController<Organization, OrganizationDto>
//    {
//        public OrganizationController(ILookupService<Organization, OrganizationDto> iLookupService, ILogger<LookupController<Organization, OrganizationDto>> logger) : base(iLookupService, logger) { }
//    }
//    public class JobController : LookupController<Job, JobDto>
//    {
//        public JobController(ILookupService<Job, JobDto> iLookupService, ILogger<LookupController<Job, JobDto>> logger) : base(iLookupService, logger) { }
//    }

//    [CheckAuthorize(
//     "Permissions.Companies.Page",
//     "Permissions.Companies.View",
//     "Permissions.Companies.Create", "Permissions.Companies.Edit", "Permissions.Companies.Delete"
// )]
//    public class CompanyController : LookupController<Company, CompanyDto>
//    {
//        public CompanyController(ILookupService<Company, CompanyDto> iLookupService, ILogger<LookupController<Company, CompanyDto>> logger) : base(iLookupService, logger) { }
//    }

//    [CheckAuthorize(
// "Permissions.Departments.Page",
// "Permissions.Departments.View",
// "Permissions.Departments.Create", "Permissions.Departments.Edit", "Permissions.Departments.Delete"
//)]
//    public class DepartmentController : LookupController<Department, DepartmentDto>
//    {
//        public DepartmentController(ILookupService<Department, DepartmentDto> iLookupService, ILogger<LookupController<Department, DepartmentDto>> logger) : base(iLookupService, logger) { }
//    }

//    [CheckAuthorize(
//"Permissions.Sections.Page",
//"Permissions.Sections.View",
//"Permissions.Sections.Create", "Permissions.Sections.Edit", "Permissions.Sections.Delete"
//)]
//    public class SectionController : LookupController<Section, SectionDto>
//    {
//        public SectionController(ILookupService<Section, SectionDto> iLookupService, ILogger<LookupController<Section, SectionDto>> logger) : base(iLookupService, logger) { }
//    }

//    [CheckAuthorize(
//"Permissions.Designations.Page",
//"Permissions.Designations.View",
//"Permissions.Designations.Create", "Permissions.Designations.Edit", "Permissions.Designations.Delete"
//)]

//    public class DesignationsController : LookupController<Designations, DesignationsDto>
//    {
//        public DesignationsController(ILookupService<Designations, DesignationsDto> iLookupService, ILogger<LookupController<Designations, DesignationsDto>> logger) : base(iLookupService, logger) { }  
//    }

//    [CheckAuthorize(
//"Permissions.Events.Page",
//"Permissions.Events.View",
//"Permissions.Events.Create", "Permissions.Events.Edit", "Permissions.Events.Delete"
//)]
//    public class EventsController : LookupController<Events, EventsDto>
//    {
//        public EventsController(ILookupService<Events, EventsDto> iLookupService, ILogger<LookupController<Events, EventsDto>> logger) : base(iLookupService, logger) { }  
//    }

//    [CheckAuthorize(
//        "Permissions.CostCenters.Page",
//        "Permissions.CostCenters.View",
//        "Permissions.CostCenters.Create", 
//        "Permissions.CostCenters.Edit", 
//        "Permissions.CostCenters.Delete"
//    )]
//    public class CostCenterController : LookupController<CostCenter, CostCenterDto>
//    {
//        public CostCenterController(ILookupService<CostCenter, CostCenterDto> iLookupService, ILogger<LookupController<CostCenter, CostCenterDto>> logger) : base(iLookupService, logger) { }
//    }

//    [CheckAuthorize(
//        "Permissions.HolidayTypes.Page",
//        "Permissions.HolidayTypes.View",
//        "Permissions.HolidayTypes.Create", 
//        "Permissions.HolidayTypes.Edit", 
//        "Permissions.HolidayTypes.Delete"
//    )]
//    public class HolidayTypeController : LookupController<HolidayTypeList, HolidayTypeDto>
//    {
//        public HolidayTypeController(ILookupService<HolidayTypeList, HolidayTypeDto> iLookupService, ILogger<LookupController<HolidayTypeList, HolidayTypeDto>> logger) : base(iLookupService, logger) { }
//    }

//    // [CheckAuthorize(
//    //     "Permissions.RamadanPeriods.Page",
//    //     "Permissions.RamadanPeriods.View",
//    //     "Permissions.RamadanPeriods.Create", 
//    //     "Permissions.RamadanPeriods.Edit", 
//    //     "Permissions.RamadanPeriods.Delete"
//    // )]
//    public class RamadanPeriodController : LookupController<RamadanPeriod, RamadanPeriodDto>
//    {
//        public RamadanPeriodController(ILookupService<RamadanPeriod, RamadanPeriodDto> iLookupService, ILogger<LookupController<RamadanPeriod, RamadanPeriodDto>> logger) : base(iLookupService, logger) { }
//    }

//}