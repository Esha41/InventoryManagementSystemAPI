using AutoMapper;
using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Orders.Mapper
{
    public class OrderMappingProfile : AutoMapper.Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderDto>()
                // Priority is recalculated fresh on every read — always accurate
                .ForMember(dest => dest.Priority,     opt => opt.MapFrom<OrderPriorityResolver>())
                // Days remaining exposed directly on DTO — frontend needs no date math
                .ForMember(dest => dest.DaysUntilDue, opt => opt.MapFrom<OrderDaysUntilDueResolver>());

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
                .ForMember(dest => dest.RequestItems, opt => opt.Ignore())
                .ForMember(dest => dest.Priority, opt => opt.Ignore());
        }
    }
}

