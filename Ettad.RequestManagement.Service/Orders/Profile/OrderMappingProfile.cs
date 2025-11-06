using AutoMapper;
using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Orders.Profile
{
    public class OrderMappingProfile : AutoMapper.Profile
    {
        public OrderMappingProfile()
        {
            // Entity to DTO - Map navigation properties to name strings
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderNo, opt => opt.MapFrom(src => src.RequestNo))
                .ForMember(dest => dest.DepartmentNameAr, opt => opt.MapFrom(src => src.Department != null ? src.Department.NameAr : null))
                .ForMember(dest => dest.DepartmentNameEn, opt => opt.MapFrom(src => src.Department != null ? src.Department.NameEn : null))
                .ForMember(dest => dest.DepotNameAr, opt => opt.MapFrom(src => src.Depot != null ? src.Depot.NameAr : null))
                .ForMember(dest => dest.DepotNameEn, opt => opt.MapFrom(src => src.Depot != null ? src.Depot.NameEn : null))
                .ForMember(dest => dest.RequestPurposeNameAr, opt => opt.MapFrom(src => src.RequestPurpose != null ? src.RequestPurpose.NameAr : null))
                .ForMember(dest => dest.RequestPurposeNameEn, opt => opt.MapFrom(src => src.RequestPurpose != null ? src.RequestPurpose.NameEn : null));

            // RequestItem to OrderRequestItemDto
            CreateMap<RequestItem, OrderRequestItemDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.Name : null))
                .ForMember(dest => dest.ItemNo, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemNo : null))
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemType : default));

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
                .ForMember(dest => dest.RequestNo, opt => opt.MapFrom(src => src.OrderNo))
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
                .ForMember(dest => dest.Reciever, opt => opt.Ignore())
                .ForMember(dest => dest.Depot, opt => opt.Ignore())
                .ForMember(dest => dest.RequestPurpose, opt => opt.Ignore())
                .ForMember(dest => dest.RequestItems, opt => opt.Ignore());

            // Update DTO to Entity (RequestItems handled manually in service)
            CreateMap<UpdateOrderDto, Order>()
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
                .ForMember(dest => dest.Reciever, opt => opt.Ignore())
                .ForMember(dest => dest.Depot, opt => opt.Ignore())
                .ForMember(dest => dest.RequestPurpose, opt => opt.Ignore())
                .ForMember(dest => dest.RequestItems, opt => opt.Ignore());
        }
    }
}

