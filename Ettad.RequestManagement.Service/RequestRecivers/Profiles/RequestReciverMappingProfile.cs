using AutoMapper;
using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.RequestRecivers.Dtos;

namespace Ettad.RequestManagement.Service.RequestRecivers.Profiles
{
    public class RequestReciverMappingProfile : Profile
    {
        public RequestReciverMappingProfile()
        {
            // Entity to DTO
            CreateMap<RequestReciver, RequestReciverDto>();

            // CreateUpdate DTO to Entity
            CreateMap<CreateUpdateRequestReciverDto, RequestReciver>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Rank, opt => opt.Ignore());
        }
    }
}

