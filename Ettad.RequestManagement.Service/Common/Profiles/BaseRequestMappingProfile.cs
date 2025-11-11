using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Common.Dtos;

namespace Ettad.RequestManagement.Service.Common.Profiles
{
    public class BaseRequestMappingProfile : AutoMapper.Profile
    {
        public BaseRequestMappingProfile()
        {
            // BaseRequest to BaseRequestDto - enums map as int by default, navigation properties as names
            CreateMap<BaseRequest, BaseRequestDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.NameEn : null))
                .ForMember(dest => dest.RequesterName, opt => opt.MapFrom(src => src.Requester != null ? (src.Requester.FullNameEN ?? src.Requester.FullNameAR ?? src.Requester.UserName) : null))
                .ForMember(dest => dest.RecieverName, opt => opt.MapFrom(src => src.Reciever != null ? (src.Reciever.FullNameEN ?? src.Reciever.FullNameAR ?? src.Reciever.UserName) : null))
                .ForMember(dest => dest.DepotName, opt => opt.MapFrom(src => src.Depot != null ? src.Depot.NameEn : null))
                .ForMember(dest => dest.RequestPurposeName, opt => opt.MapFrom(src => src.RequestPurpose != null ? src.RequestPurpose.NameEn : null));

            // RequestItem to RequestItemDto - navigation properties as names
            CreateMap<RequestItem, RequestItemDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.Name : null))
                .ForMember(dest => dest.ItemNo, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemNo : null));
        }
    }
}
