using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Inventories.Dtos;
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
            CreateMap<InventoryDetailEntity, InventoryDetailDto>();

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
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InventoryId, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentQuantity, opt => opt.MapFrom(src => src.ItemQuantity)) // Initially current = item quantity
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
                .ForMember(dest => dest.InventoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Inventory, opt => opt.Ignore())
                .ForMember(dest => dest.Supplier, opt => opt.Ignore())
                .ForMember(dest => dest.Manufacturer, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore());
        }
    }
}

