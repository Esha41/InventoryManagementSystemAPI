using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Common.Dtos;

namespace Ettad.RequestManagement.Service.Common.Profiles
{
    public class BaseRequestMappingProfile : AutoMapper.Profile
    {
        public BaseRequestMappingProfile()
        {
            // BaseRequest to BaseRequestDto - handles enum to string conversions and navigation properties as names
            CreateMap<BaseRequest, BaseRequestDto>()
                .ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.RequestType.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.NameEn : null))
                .ForMember(dest => dest.RequesterName, opt => opt.MapFrom(src => src.Requester != null ? src.Requester.NameEn : null))
                .ForMember(dest => dest.RecieverName, opt => opt.MapFrom(src => src.Reciever != null ? src.Reciever.NameEn : null))
                .ForMember(dest => dest.DepotName, opt => opt.MapFrom(src => src.Depot != null ? src.Depot.NameEn : null))
                .ForMember(dest => dest.RequestPurposeName, opt => opt.MapFrom(src => src.RequestPurpose != null ? src.RequestPurpose.NameEn : null));

            // RequestItem to RequestItemDto - navigation properties as names
            CreateMap<RequestItem, RequestItemDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.Name : null))
                .ForMember(dest => dest.ItemNo, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemNo : null));
        }
    }
}
