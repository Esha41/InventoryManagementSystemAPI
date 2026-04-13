using System.Collections.Generic;
using System.Linq;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Common.Dtos;

namespace Ettad.Inventory.Service.Common.Profiles
{
    public class BaseItemMappingProfile : AutoMapper.Profile
    {
        public BaseItemMappingProfile()
        {
            CreateMap<BaseItem, BaseItemDto>()
                .ForMember(dest => dest.PrimaryPurposes, opt => opt.MapFrom(src =>
                    src.BaseItemPrimaryPurposes == null
                        ? new List<PrimaryPurpos>()
                        : src.BaseItemPrimaryPurposes.Where(x => x.PrimaryPurpos != null).Select(x => x.PrimaryPurpos).ToList()));

            CreateMap<Ammunition, BaseItemDto>()
                .IncludeBase<BaseItem, BaseItemDto>();
            CreateMap<Weapon, BaseItemDto>()
                .IncludeBase<BaseItem, BaseItemDto>();
            CreateMap<Explosive, BaseItemDto>()
                .IncludeBase<BaseItem, BaseItemDto>();
            CreateMap<Accessory, BaseItemDto>()
                .IncludeBase<BaseItem, BaseItemDto>();
        }
    }
}
