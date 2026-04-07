using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Inventory.Service.Common.Dtos;
using Ettad.Module.lookup.Dtos;
using Ettad.User.Services.DTO;
using Ettad.Comman.Idenitity;

namespace Ettad.Inventory.Service.Assets.Profiles
{
    public class AssetMappingProfile : Profile
    {
        public AssetMappingProfile()
        {
            // Map ApplicationUser to UserDto (simplified mapping - roles are populated separately in UserService)
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName ?? string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
                .ForMember(dest => dest.IsLdapUser, opt => opt.MapFrom(src => src.IsLdapUser))
                .ForMember(dest => dest.IsSuperAdmin, opt => opt.MapFrom(src => src.IsSuperAdmin))
                .ForMember(dest => dest.ExtraEmployeesView, opt => opt.MapFrom(src => src.ExtraEmployeesView ?? string.Empty))
                .ForMember(dest => dest.DeparmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.FullNameEN, opt => opt.MapFrom(src => src.FullNameEN ?? string.Empty))
                .ForMember(dest => dest.FullNameAR, opt => opt.MapFrom(src => src.FullNameAR ?? string.Empty))
                .ForMember(dest => dest.RankId, opt => opt.MapFrom(src => src.RankId))
                .ForMember(dest => dest.MilitoryId, opt => opt.MapFrom(src => src.MilitoryId))
                .ForMember(dest => dest.LdapUserName, opt => opt.MapFrom(src => src.LdapUserName ?? string.Empty))
                .ForMember(dest => dest.Roles, opt => opt.Ignore()) // Roles are populated separately
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? src.Department : null))
                .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Rank != null ? src.Rank : null));

            CreateMap<Asset, AssetDto>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item != null ? src.Item : null))
                .ForMember(dest => dest.Depot, opt => opt.MapFrom(src => src.Depot != null ? src.Depot : null))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.CurrentAssignment != null && src.CurrentAssignment.Department != null ? src.CurrentAssignment.Department : null))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.CurrentAssignment != null ? src.CurrentAssignment.DepartmentId : null))
                .ForMember(dest => dest.Custodian, opt => opt.MapFrom(src => src.CurrentAssignment != null && src.CurrentAssignment.Custodian != null ? src.CurrentAssignment.Custodian : null))
                .ForMember(dest => dest.CustodianId, opt => opt.MapFrom(src => src.CurrentAssignment != null ? src.CurrentAssignment.CustodianId : null))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.CurrentAssignment != null ? src.CurrentAssignment.Location : null))
                .ForMember(dest => dest.BatchNumber, opt => opt.MapFrom(src => src.Batch != null ? src.Batch.BatchNumber : string.Empty))
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<CreateAssetDto, Asset>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.Depot, opt => opt.Ignore())
                .ForMember(dest => dest.Batch, opt => opt.Ignore())
                .ForMember(dest => dest.BatchId, opt => opt.Ignore())
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
                .ForMember(dest => dest.Batch, opt => opt.Ignore())
                .ForMember(dest => dest.BatchId, opt => opt.Ignore())
                .ForMember(dest => dest.DepotId, opt => opt.Ignore());
        }
    }
}

