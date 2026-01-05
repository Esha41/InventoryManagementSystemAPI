using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.AssetSupply.Dtos;

namespace Ettad.Inventory.Service.AssetSupply.Profiles
{
    public class AssetSupplyMappingProfile : Profile
    {
        public AssetSupplyMappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<Ettad.Data.Entities.AssetSupply, AssetSupplyDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.NameEn : null))
                .ForMember(dest => dest.CustodianName, opt => opt.MapFrom(src => src.Custodian != null ? src.Custodian.NameEn : null))
                .ForMember(dest => dest.ReceiverRankName, opt => opt.MapFrom(src => src.ReceiverRank != null ? src.ReceiverRank.NameEn : null));

            CreateMap<AssetSupplyDetail, AssetSupplyDetailDto>()
                .ForMember(dest => dest.AssetSerialNumber, opt => opt.MapFrom(src => src.Asset != null ? src.Asset.SerialNumber : null))
                .ForMember(dest => dest.AssetTag, opt => opt.MapFrom(src => src.Asset != null ? src.Asset.AssetTag : null))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item != null ? src.Item.Name : null));

            // DTO to Entity mappings for Create
            CreateMap<CreateAssetSupplyDto, Ettad.Data.Entities.AssetSupply>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.SupplyDate, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverName, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverMilitaryId, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverRankId, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Custodian, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverRank, opt => opt.Ignore())
                .ForMember(dest => dest.SupplyDetails, opt => opt.Ignore())
                .ForMember(dest => dest.Assignments, opt => opt.Ignore());

            CreateMap<CreateAssetSupplyDetailDto, AssetSupplyDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AssetSupplyId, opt => opt.Ignore())
                .ForMember(dest => dest.ItemId, opt => opt.Ignore())
                .ForMember(dest => dest.SequenceNo, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelivered, opt => opt.Ignore())
                .ForMember(dest => dest.DeliveredDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.AssetSupply, opt => opt.Ignore())
                .ForMember(dest => dest.Asset, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore());

            // Asset to AssetToSupplyDto
            CreateMap<Asset, AssetToSupplyDto>()
                .ForMember(dest => dest.DepotName, opt => opt.MapFrom(src => src.Depot != null ? src.Depot.NameEn : null))
                .ForMember(dest => dest.Priority, opt => opt.Ignore());
        }
    }
}

