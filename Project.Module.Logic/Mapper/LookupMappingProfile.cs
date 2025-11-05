using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Module.lookup.Mapper
{
    public class LookupMappingProfile : Profile
    {
        public LookupMappingProfile()
        {
            // CreateUpdate DTOs to Entities
            CreateMap<CreateUpdateDepartmentDto, Department>();
            CreateMap<CreateUpdateCountryDto, Country>();
            CreateMap<CreateUpdateDepotDto, Depot>();
            CreateMap<CreateUpdateColorDto, Color>();
            CreateMap<CreateUpdateCaseTypeDto, CaseType>();
            CreateMap<CreateUpdateCompatibilityDto, Compatibility>();
            CreateMap<CreateUpdateHazardDivisionDto, HazardDivision>();
            CreateMap<CreateUpdateHccDto, Hcc>();
            CreateMap<CreateUpdateManufacturerDto, Manufacturer>();
            CreateMap<CreateUpdateNatureOptionDto, NatureOption>();
            CreateMap<CreateUpdateNsnDto, Nsn>();
            CreateMap<CreateUpdatePrimaryPurposDto, PrimaryPurpos>();
            CreateMap<CreateUpdateProjectailMaterialDto, ProjectailMaterial>();
            CreateMap<CreateUpdatePropellantDto, Propellant>();
            CreateMap<CreateUpdateUnitDto, Unit>();
            CreateMap<CreateUpdateSupplierDto, Supplier>();

            // Entities to Read-Only DTOs
            CreateMap<CaseType, CaseTypeDto>();
            CreateMap<Color, ColorDto>();
            CreateMap<Compatibility, CompatibilityDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Department, DepartmentDto>();
            CreateMap<Depot, DepotDto>();
            CreateMap<Hcc, HccDto>();
            CreateMap<HazardDivision, HazardDivisionDto>();
            CreateMap<Manufacturer, ManufacturerDto>();
            CreateMap<NatureOption, NatureOptionDto>();
            CreateMap<Nsn, NsnDto>();
            CreateMap<PrimaryPurpos, PrimaryPurposDto>();
            CreateMap<ProjectailMaterial, ProjectailMaterialDto>();
            CreateMap<Propellant, PropellantDto>();
            CreateMap<Supplier, SupplierDto>();
            CreateMap<Unit, UnitDto>();
            CreateMap<CreateUpdateRankDto, Rank>();
            CreateMap<CreateUpdateWorkFlowTypeDto, WorkFlowType>();
        }
    }
}
