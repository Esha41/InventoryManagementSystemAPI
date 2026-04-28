using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Weapons.Dtos;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Weapons.Mapper
{
    public class WeaponMappingProfile : Profile
    {
        public WeaponMappingProfile()
        {
            CreateMap<Caliber, CaliberDto>();

            CreateMap<Weapon, WeaponDto>()
                .ForMember(dest => dest.Caliber, opt => opt.MapFrom(src => src.LookupCaliber))
                .ForMember(dest => dest.PrimaryPurposes, opt => opt.MapFrom(src =>
                    src.BaseItemPrimaryPurposes != null
                        ? src.BaseItemPrimaryPurposes.Select(x => x.PrimaryPurpos).ToList()
                        : new List<PrimaryPurpos>()));

            CreateMap<CreateUpdateWeaponDto, Weapon>()
                .ForMember(dest => dest.CaliberCategory, opt => opt.Ignore())
                .ForMember(dest => dest.LookupCaliber, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ItemType, opt => opt.Ignore())
                .ForMember(dest => dest.CaliberUnit, opt => opt.Ignore())
                .ForMember(dest => dest.CountryOfManufacture, opt => opt.Ignore())
                .ForMember(dest => dest.BaseItemPrimaryPurposes, opt => opt.Ignore())
                .ForMember(dest => dest.Classification, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.Ignore());
        }
    }
}
