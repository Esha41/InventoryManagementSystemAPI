using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.AllowanceItems.Dtos;

namespace Ettad.Inventory.Service.AllowanceItems.Profiles
{
    public class AllowanceItemMappingProfile : Profile
    {
        public AllowanceItemMappingProfile()
        {
            // Entity to DTO
            CreateMap<AllowanceItem, AllowanceItemDto>();

            // CreateUpdate DTO to Entity
            CreateMap<CreateUpdateAllowanceItemDto, AllowanceItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore());
        }
    }
}

