using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Explosives.Dtos;

namespace Ettad.Inventory.Service.Explosives.Profiles
{
    public class ExplosiveMappingProfile : Profile
    {
        public ExplosiveMappingProfile()
        {
            CreateMap<Explosive, ExplosiveDto>();

            CreateMap<CreateUpdateExplosiveDto, Explosive>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ItemType, opt => opt.Ignore())
                .ForMember(dest => dest.NetExplosiveQuantityUnit, opt => opt.Ignore())
                .ForMember(dest => dest.TotalWeightUnit, opt => opt.Ignore())
                .ForMember(dest => dest.HazardDivision, opt => opt.Ignore())
                .ForMember(dest => dest.Compatibility, opt => opt.Ignore());
        }
    }
}
