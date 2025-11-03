using AutoMapper;
using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Requests.Dtos;
using Entities = Ettad.Data.Entities;

namespace Ettad.RequestManagement.Service.Requests.Profiles
{
    public class RequestMappingprofile : Profile
    {
        public RequestMappingprofile()
        {
            // Entity to DTO
            CreateMap<Request, RequestDto>();

            // RequestDetail Entity to DTO
            CreateMap<RequestDetail, RequestDetailDto>();

            // CreateUpdateRequestDetailDto to RequestDetail (used for mapping)
            CreateMap<CreateUpdateRequestDetailDto, RequestDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore());

            //// CreateUpdate DTO to Entity
            CreateMap<CreateUpdateRequestDto, Request>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ResquestDetails, opt => opt.Ignore()); // We handle this manually in the service
        }
    }
}
