using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.WeaponAccessories.Dtos;

namespace Ettad.Inventory.Service.WeaponAccessories.Mapper
{
    public class WeaponAccessoryMappingProfile : Profile
    {
        public WeaponAccessoryMappingProfile()
        {
            CreateMap<WeaponAccessory, WeaponAccessoryDto>()
                .ForMember(dest => dest.Accessory, opt => opt.MapFrom(src => src.Accessory == null ? null : new WeaponAccessoryItemSummaryDto
                {
                    Id = src.Accessory.Id,
                    ItemNo = src.Accessory.ItemNo,
                    Name = src.Accessory.Name,
                    NameAr = src.Accessory.NameAr
                }))
                .ForMember(dest => dest.Weapon, opt => opt.MapFrom(src => src.Weapon == null ? null : new WeaponAccessoryItemSummaryDto
                {
                    Id = src.Weapon.Id,
                    ItemNo = src.Weapon.ItemNo,
                    Name = src.Weapon.Name,
                    NameAr = src.Weapon.NameAr
                }));

            CreateMap<CreateUpdateWeaponAccessoryDto, WeaponAccessory>()
                .ForMember(dest => dest.Weapon, opt => opt.Ignore())
                .ForMember(dest => dest.Accessory, opt => opt.Ignore());
        }
    }
}
