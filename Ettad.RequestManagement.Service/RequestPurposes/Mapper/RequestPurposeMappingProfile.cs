using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.RequestPurposes.Dtos;

namespace Ettad.RequestManagement.Service.RequestPurposes.Mapper
{
    public class RequestPurposeMappingProfile : Profile
    {
        public RequestPurposeMappingProfile()
        {
            // Entity to DTO - enum maps as int by default
            CreateMap<RequestPurpose, RequestPurposeDto>();

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

