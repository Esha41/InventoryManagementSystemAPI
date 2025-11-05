using AutoMapper;
using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Orders.Profile
{
    public class OrderMappingProfile : AutoMapper.Profile
    {
        public OrderMappingProfile()
        {
            // Entity to DTO
            CreateMap<Order, OrderDto>();

            // CreateUpdate DTO to Entity
            CreateMap<CreateUpdateOrderDto, Order>()
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

