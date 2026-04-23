using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Explosives.Dtos;

namespace Ettad.Inventory.Service.Explosives.Mapper
{
    public class ExplosiveMappingProfile : Profile
    {
        public ExplosiveMappingProfile()
        {
            CreateMap<Explosive, ExplosiveDto>()
                .ForMember(dest => dest.PrimaryPurposes, opt => opt.MapFrom(src =>
                    src.BaseItemPrimaryPurposes != null
                        ? src.BaseItemPrimaryPurposes.Select(x => x.PrimaryPurpos).ToList()
                        : new List<PrimaryPurpos>()));

            CreateMap<CreateUpdateExplosiveDto, Explosive>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ItemType, opt => opt.Ignore())
                .ForMember(dest => dest.Compatibility, opt => opt.Ignore())
                .ForMember(dest => dest.HazardDivision, opt => opt.Ignore())
                .ForMember(dest => dest.Classification, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.Ignore())
                .ForMember(dest => dest.Unit, opt => opt.Ignore())
                .ForMember(dest => dest.BaseItemPrimaryPurposes, opt => opt.Ignore());
        }
    }
}
