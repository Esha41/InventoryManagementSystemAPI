using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.Module.lookup.Dtos;

namespace Ettad.RequestManagement.Service.Common.Profiles
{
    public class BaseRequestMappingProfile : AutoMapper.Profile
    {
        public BaseRequestMappingProfile()
        {
            // BaseRequest to BaseRequestDto - handles enum to string conversions
            CreateMap<BaseRequest, BaseRequestDto>()
                .ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.RequestType.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<RequestItem, RequestItemDto>();
            
            // Map RequestPurpose entity to RequestPurposeDto
            CreateMap<RequestPurpose, RequestPurposeDto>()
                .ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.RequestType.ToString()));
        }
    }
}
