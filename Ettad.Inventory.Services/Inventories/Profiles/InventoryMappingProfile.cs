using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.Inventory.Service.Common.Dtos;
using InventoryEntity = Ettad.Data.Entities.Inventory;
using InventoryDetailEntity = Ettad.Data.Entities.InventoryDetail;

namespace Ettad.Inventory.Service.Inventories.Profiles
{
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile()
        {
            // Entity to DTO
            CreateMap<InventoryEntity, InventoryDto>();
            CreateMap<InventoryDetailEntity, InventoryDetailDto>()
                .ForMember(dest => dest.OriginalQuantity, opt => opt.MapFrom(src => src.ItemQuantity));
            CreateMap<InventoryDetailEntity, LotDetailDto>()
                .ForMember(dest => dest.InventoryDetailId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
                .ForMember(dest => dest.OriginalQuantity, opt => opt.MapFrom(src => src.ItemQuantity))
                .ForMember(dest => dest.UsedQuantity, opt => opt.Ignore()) // Will be set in service
                .ForMember(dest => dest.RemainingQuantity, opt => opt.Ignore()) // Will be set in service
                .ForMember(dest => dest.IsEmptyLot, opt => opt.Ignore()) // Will be set in service
                .ForMember(dest => dest.IsExpired, opt => opt.Ignore()) // Will be set in service
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.Item.ExpiryDate))
                .ForMember(dest => dest.Depot, opt => opt.MapFrom(src => src.Inventory.Depo));
            
            // Create DTO to Entity
            CreateMap<CreateInventoryDto, InventoryEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Depo, opt => opt.Ignore());

            CreateMap<CreateInventoryDetailDto, InventoryDetailEntity>()
                .ForMember(dest => dest.ItemQuantity, opt => opt.MapFrom(src => src.OriginalQuantity))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InventoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Inventory, opt => opt.Ignore())
                .ForMember(dest => dest.Supplier, opt => opt.Ignore())
                .ForMember(dest => dest.Manufacturer, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore());

            // Update DTO to Entity
            CreateMap<UpdateInventoryDto, InventoryEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Depo, opt => opt.Ignore())
                .ForMember(dest => dest.InventoryDetails, opt => opt.Ignore()); // Handle separately

            CreateMap<UpdateInventoryDetailDto, InventoryDetailEntity>()
                .ForMember(dest => dest.ItemQuantity, opt => opt.MapFrom(src => src.OriginalQuantity))
                .ForMember(dest => dest.InventoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Inventory, opt => opt.Ignore())
                .ForMember(dest => dest.Supplier, opt => opt.Ignore())
                .ForMember(dest => dest.Manufacturer, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore());
        }
    }
}

