using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.AssetHistory.Dtos;

namespace Ettad.Inventory.Service.AssetHistory.Mapper
{
    public class AssetHistoryMappingProfile : Profile
    {
        public AssetHistoryMappingProfile()
        {
            CreateMap<Ettad.Data.Entities.AssetHistory, AssetHistoryDto>()
                .ForMember(dest => dest.AssetSerialNumber, opt => opt.MapFrom(src => src.Asset != null ? src.Asset.SerialNumber : null))
                .ForMember(dest => dest.PreviousDepartmentName, opt => opt.MapFrom(src => src.PreviousDepartment != null ? src.PreviousDepartment.NameEn : null))
                .ForMember(dest => dest.NewDepartmentName, opt => opt.MapFrom(src => src.NewDepartment != null ? src.NewDepartment.NameEn : null))
                .ForMember(dest => dest.PreviousCustodianName, opt => opt.MapFrom(src => src.PreviousCustodian != null ? src.PreviousCustodian.NameEn : null))
                .ForMember(dest => dest.NewCustodianName, opt => opt.MapFrom(src => src.NewCustodian != null ? src.NewCustodian.NameEn : null))
                .ForMember(dest => dest.OrderRequestNo, opt => opt.MapFrom(src => src.Order != null ? src.Order.RequestNo : null));
        }
    }
}

