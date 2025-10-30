using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Services.Ammunitions.Dtos;

namespace Ettad.Inventory.Services.Ammunitions.Profiles
{
    public class AmmunitionMappingProfile : Profile
    {
        public AmmunitionMappingProfile()
        {
            // Entity to DTO
            CreateMap<Ammunition, AmmunitionDto>()
                .ReverseMap();

            // CreateUpdate DTO to Entity
            CreateMap<CreateUpdateAmmunitionDto, Ammunition>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ItemType, opt => opt.Ignore())
                .ForMember(dest => dest.Hcc, opt => opt.Ignore())
                .ForMember(dest => dest.Supplier, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Manufacturer, opt => opt.Ignore())
                .ForMember(dest => dest.BulletDiameterUnit, opt => opt.Ignore())
                .ForMember(dest => dest.CaseLengthUnit, opt => opt.Ignore())
                .ForMember(dest => dest.NatureOption, opt => opt.Ignore())
                .ForMember(dest => dest.Nsn, opt => opt.Ignore())
                .ForMember(dest => dest.PrimaryPurpos, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectileColor, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectailMaterial, opt => opt.Ignore())
                .ForMember(dest => dest.CaseType, opt => opt.Ignore())
                .ForMember(dest => dest.Propellant, opt => opt.Ignore())
                .ForMember(dest => dest.Compatibility, opt => opt.Ignore())
                .ForMember(dest => dest.HazardDivision, opt => opt.Ignore());
        }
    }
}

