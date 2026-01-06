using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Assets.Dtos;

namespace Ettad.Inventory.Service.Assets.Profiles
{
    public class AssetMappingProfile : Profile
    {
        public AssetMappingProfile()
        {
            CreateMap<Asset, AssetDto>()
                .ForMember(dest => dest.Custodian, opt => opt.MapFrom(src => src.CurrentAssignment != null ? src.CurrentAssignment.Custodian : null))
                .ForMember(dest => dest.CustodianId, opt => opt.MapFrom(src => src.CurrentAssignment != null ? src.CurrentAssignment.CustodianId : null))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.CurrentAssignment != null ? src.CurrentAssignment.DepartmentId : null))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.CurrentAssignment != null ? src.CurrentAssignment.Location : null));

            CreateMap<CreateAssetDto, Asset>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Depot, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<UpdateAssetDto, Asset>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Depot, opt => opt.Ignore())
                .ForMember(dest => dest.DepotId, opt => opt.Ignore());
        }
    }
}

