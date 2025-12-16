using AutoMapper;
using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Orders.Profile
{
    public class OrderMappingProfile : AutoMapper.Profile
    {
        public OrderMappingProfile()
        {
            // Entity to DTO - Map navigation properties to name strings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.RequestNo, opt => opt.MapFrom(src => src.RequestNo))
                .ForMember(dest => dest.DepartmentNameAr, opt => opt.MapFrom(src => src.Department != null ? src.Department.NameAr : null))
                .ForMember(dest => dest.DepartmentNameEn, opt => opt.MapFrom(src => src.Department != null ? src.Department.NameEn : null))
                .ForMember(dest => dest.RequesterName, opt => opt.MapFrom(src => src.Requester != null ? (src.Requester.FullNameEN ?? src.Requester.FullNameAR ?? src.Requester.UserName) : null))
                .ForMember(dest => dest.RequestPurposeNameAr, opt => opt.MapFrom(src => src.RequestPurpose != null ? src.RequestPurpose.NameAr : null))
                .ForMember(dest => dest.RequestPurposeNameEn, opt => opt.MapFrom(src => src.RequestPurpose != null ? src.RequestPurpose.NameEn : null))
                .ForMember(dest => dest.RequestItems, opt => opt.MapFrom(src => src.RequestItems != null ? src.RequestItems.Where(ri => !ri.IsDeleted) : null));

            // Order to BaseRequestDto - includes Order-specific fields for unified API
            CreateMap<Order, BaseRequestDto>()
                .IncludeBase<BaseRequest, BaseRequestDto>()
                .ForMember(dest => dest.UsageDateFrom, opt => opt.MapFrom(src => src.UsageDateFrom))
                .ForMember(dest => dest.UsageDateTo, opt => opt.MapFrom(src => src.UsageDateTo))
                .ForMember(dest => dest.UsageTimeFrom, opt => opt.MapFrom(src => src.UsageTimeFrom))
                .ForMember(dest => dest.UsageTimeTo, opt => opt.MapFrom(src => src.UsageTimeTo))
                .ForMember(dest => dest.UsagePurpose, opt => opt.MapFrom(src => src.UsagePurpose))
                .ForMember(dest => dest.UsageLocation, opt => opt.MapFrom(src => src.UsageLocation))
                .ForMember(dest => dest.IsFromAllowance, opt => opt.MapFrom(src => src.IsFromAllowance))
                .ForMember(dest => dest.AnnualDiscard, opt => opt.MapFrom(src => src.AnnualDiscard != null ? (bool?)(src.AnnualDiscard > 0) : null))
                .ForMember(dest => dest.NumberOfOfficer, opt => opt.MapFrom(src => src.NumberOfOfficer))
                .ForMember(dest => dest.NumberOfOtherRank, opt => opt.MapFrom(src => src.NumberOfOtherRank));




            // RequestItem to OrderRequestItemDto
            // Inherits all mappings from RequestItem -> RequestItemDto (including ItemType)
            CreateMap<RequestItem, OrderRequestItemDto>()
                .IncludeBase<RequestItem, RequestItemDto>();


            // CreateUpdateRequestItemDto to RequestItem
            CreateMap<CreateUpdateRequestItemDto, RequestItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore());

            // Create DTO to Entity (RequestItems handled manually in service)
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Requester, opt => opt.Ignore())
                .ForMember(dest => dest.RequestPurpose, opt => opt.Ignore())
                .ForMember(dest => dest.RequestItems, opt => opt.Ignore());
        }
    }
}

