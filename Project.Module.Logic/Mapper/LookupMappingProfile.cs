using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Module.lookup.Mapper
{
    public class LookupMappingProfile : Profile
    {
        public LookupMappingProfile()
        {
            CreateMap<DepartmentDto, Department>();
            CreateMap<CountryDto, Country>();
            CreateMap<DepotDto, Depot>();
            CreateMap<ColorDto, Color>();
            CreateMap<CaseTypeDto, CaseType>();
            CreateMap<CompatibilityDto, Compatibility>();
            CreateMap<HazardDivisionDto, HazardDivision>();
            CreateMap<HccDto, Hcc>();
            CreateMap<ManufacturerDto, Manufacturer>();
            CreateMap<NatureOptionDto, NatureOption>();
            CreateMap<NsnDto, Nsn>();
            CreateMap<PrimaryPurposDto, PrimaryPurpos>();
            CreateMap<ProjectailMaterialDto, ProjectailMaterial>();
            CreateMap<PropellantDto, Propellant>();
            CreateMap<UnitDto, Unit>();
            CreateMap<SupplierDto, Supplier>();
        }
    }
}
