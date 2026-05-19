using System.Collections.Generic;
using System.Linq;
using Ettad.Comman.Idenitity;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.RequestManagement.Service.Discards.Dtos;
using Ettad.RequestManagement.Service.Returns.Dtos;

namespace Ettad.RequestManagement.Service.Common.Mapper
{
    public class BaseRequestMappingProfile : AutoMapper.Profile
    {
        public BaseRequestMappingProfile()
        {
            // BaseRequest to BaseRequestDto
            CreateMap<BaseRequest, BaseRequestDto>()
                .Include<Order, OrderDto>()
                .Include<Discard, DiscardDto>()
                .Include<Return, ReturnDto>()
                .ForMember(dest => dest.AutoRejectedAt, opt => opt.MapFrom(
                    src => src.Status == RequestStatus.AutoRejected ? src.ModificationDate : (DateTime?)null));

            // RequestItem to RequestItemDto - navigation properties as names
            CreateMap<RequestItemWeaponAssociation, RequestItemWeaponAssociationDto>();

            CreateMap<RequestItem, RequestItemDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.Name : null))
                .ForMember(dest => dest.ItemNameAr, opt => opt.MapFrom(src => src.Item != null ? src.Item.NameAr : null))
                .ForMember(dest => dest.ItemNo, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemNo : null))
                .ForMember(dest => dest.Nsn, opt => opt.MapFrom(src => src.Item != null ? src.Item.Nsn : null))
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemType : default))
                .ForMember(dest => dest.WeaponAssociations, opt => opt.MapFrom(src =>
                    src.WeaponAssociations == null
                        ? new List<RequestItemWeaponAssociation>()
                        : src.WeaponAssociations.Where(w => !w.IsDeleted).ToList()));

            // Map ApplicationUser to RequesterDto
            CreateMap<ApplicationUser, RequesterDto>();
        }
    }
}
