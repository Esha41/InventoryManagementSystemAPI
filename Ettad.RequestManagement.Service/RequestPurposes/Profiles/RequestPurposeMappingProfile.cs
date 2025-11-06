using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.RequestPurposes.Dtos;

namespace Ettad.RequestManagement.Service.RequestPurposes.Profiles
{
    public class RequestPurposeMappingProfile : Profile
    {
        public RequestPurposeMappingProfile()
        {
            CreateMap<RequestPurpose, RequestPurposeDto>()
                .ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.RequestType.ToString()));

            CreateMap<CreateUpdateRequestPurposeDto, RequestPurpose>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestType, opt => opt.Ignore()) // Set by service
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());
        }
    }
}

