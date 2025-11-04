using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Module.lookup.Mapper
{
    public class LookupMappingProfile : Profile
    {
        public LookupMappingProfile()
        {
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
            CreateMap<CreateUpdateRankDto, Rank>();
        }
    }
}
