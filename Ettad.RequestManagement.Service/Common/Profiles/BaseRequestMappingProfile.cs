using Ettad.Comman.Idenitity;
using Ettad.Data.Entities;
using Ettad.Module.lookup.Dtos;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.RequestManagement.Service.RequestPurposes.Dtos;

namespace Ettad.RequestManagement.Service.Common.Profiles
{
    public class BaseRequestMappingProfile : AutoMapper.Profile
    {
        public BaseRequestMappingProfile()
        {
            // BaseRequest to BaseRequestDto
            CreateMap<BaseRequest, BaseRequestDto>();

            // RequestItem to RequestItemDto - navigation properties as names
            CreateMap<RequestItem, RequestItemDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.Name : null))
                .ForMember(dest => dest.ItemNo, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemNo : null))
                .ForMember(dest => dest.Nsn, opt => opt.MapFrom(src => src.Item != null ? src.Item.Nsn : null));

            // Map ApplicationUser to RequesterDto
            CreateMap<ApplicationUser, RequesterDto>();
        }
    }
}
