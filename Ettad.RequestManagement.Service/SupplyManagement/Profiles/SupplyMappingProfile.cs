using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.Inventory.Service.Common.Dtos;
using Ettad.Module.lookup.Dtos;

namespace Ettad.RequestManagement.Service.SupplyManagement.Profiles
{
    public class SupplyMappingProfile : Profile
    {
        public SupplyMappingProfile()
        {
            // Entity to DTO
            CreateMap<Supply, SupplyDto>()
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.ReceiverEmployee, opt => opt.MapFrom(src => src.ReceiverEmployee));

            CreateMap<SupplyDetail, SupplyDetailDto>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item))
                .ForMember(dest => dest.RequestedQuantity, opt => opt.Ignore()) // Will be set in service
                .ForMember(dest => dest.TotalSuppliedQuantity, opt => opt.Ignore()) // Will be set in service
                .ForMember(dest => dest.IsFullyFulfilled, opt => opt.Ignore()); // Will be set in service

            // Create DTO to Entity
            CreateMap<CreateSupplyDto, Supply>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.SubmissionStatus, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.FulfillmentStatus, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverEmployee, opt => opt.Ignore())
                .ForMember(dest => dest.SupplyDetails, opt => opt.Ignore()); // Handle separately

            CreateMap<CreateSupplyDetailDto, SupplyDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SupplyId, opt => opt.Ignore())
                .ForMember(dest => dest.Supply, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Lot, opt => opt.MapFrom(src => (src.Lot ?? string.Empty).Trim()));

            // Update DTO to Entity
            CreateMap<UpdateSupplyDto, Supply>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.SubmissionStatus, opt => opt.Ignore())
                .ForMember(dest => dest.FulfillmentStatus, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverEmployee, opt => opt.Ignore())
                .ForMember(dest => dest.SupplyDetails, opt => opt.Ignore());

            CreateMap<UpdateSupplyDetailDto, SupplyDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SupplyId, opt => opt.Ignore())
                .ForMember(dest => dest.Supply, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Lot, opt => opt.MapFrom(src => (src.Lot ?? string.Empty).Trim()));
        }
    }
}

