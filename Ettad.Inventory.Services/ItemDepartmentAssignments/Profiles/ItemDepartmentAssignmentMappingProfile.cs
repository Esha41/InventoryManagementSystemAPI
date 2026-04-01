using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Dtos;

namespace Ettad.Inventory.Service.ItemDepartmentAssignments.Profiles
{
    public class ItemDepartmentAssignmentMappingProfile : Profile
    {
        public ItemDepartmentAssignmentMappingProfile()
        {
            // Entity to DTO
            CreateMap<ItemDepartmentAssignment, ItemDepartmentAssignmentDto>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.Name : null))
                .ForMember(dest => dest.ItemNo, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemNo : null))
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.Item != null ? src.Item.ItemType : default))
                .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => src.Department != null ? src.Department.Code : null))
                .ForMember(dest => dest.DepartmentNameAr, opt => opt.MapFrom(src => src.Department != null ? src.Department.NameAr : null))
                .ForMember(dest => dest.DepartmentNameEn, opt => opt.MapFrom(src => src.Department != null ? src.Department.NameEn : null));

            // CreateUpdate DTO to Entity
            CreateMap<CreateUpdateItemDepartmentAssignmentDto, ItemDepartmentAssignment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore());
        }
    }
}
