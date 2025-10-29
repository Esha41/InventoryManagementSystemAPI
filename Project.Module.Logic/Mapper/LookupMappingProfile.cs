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
        }
    }
}
