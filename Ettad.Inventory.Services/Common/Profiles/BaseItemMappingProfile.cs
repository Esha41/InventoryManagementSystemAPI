using Ettad.Data.Entities;
using Ettad.Inventory.Service.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Inventory.Service.Common.Profiles
{
    public class BaseItemMappingProfile : AutoMapper.Profile
    {
        public BaseItemMappingProfile()
        {
            // BaseItem to BaseItemDto mapping (for polymorphic mapping from Ammunition, Explosive, Weapon, Accessory)
            CreateMap<BaseItem, BaseItemDto>();
        }
    }
}
