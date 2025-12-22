using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Weapons.Dtos;

namespace Ettad.Inventory.Service.Weapons.Profiles
{
    public class WeaponMappingProfile : Profile
    {
        public WeaponMappingProfile()
        {
            CreateMap<Weapon, WeaponDto>();

            CreateMap<CreateUpdateWeaponDto, Weapon>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ItemType, opt => opt.Ignore())
                .ForMember(dest => dest.BarrelLengthUnit, opt => opt.Ignore())
                .ForMember(dest => dest.OverallLengthUnit, opt => opt.Ignore())
                .ForMember(dest => dest.WeightUnit, opt => opt.Ignore());
        }
    }
}
