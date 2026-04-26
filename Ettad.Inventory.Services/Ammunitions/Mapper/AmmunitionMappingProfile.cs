using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Ammunitions.Dtos;

namespace Ettad.Inventory.Service.Ammunitions.Mapper
{
    public class AmmunitionMappingProfile : Profile
    {
        public AmmunitionMappingProfile()
        {
            CreateMap<Ammunition, AmmunitionDto>()
                .ForMember(dest => dest.PrimaryPurposes, opt => opt.MapFrom(src =>
                    src.BaseItemPrimaryPurposes != null
                        ? src.BaseItemPrimaryPurposes.Select(x => x.PrimaryPurpos).ToList()
                        : new List<PrimaryPurpos>()));

            // CreateUpdate DTO to Entity
            CreateMap<CreateUpdateAmmunitionDto, Ammunition>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModificationDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ItemType, opt => opt.Ignore())
                .ForMember(dest => dest.AmmunitionType, opt => opt.Ignore())
                .ForMember(dest => dest.BulletDiameterUnit, opt => opt.Ignore())
                .ForMember(dest => dest.NatureOption, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectileColor, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectailMaterial, opt => opt.Ignore())
                .ForMember(dest => dest.CaseType, opt => opt.Ignore())
                .ForMember(dest => dest.Propellant, opt => opt.Ignore())
                .ForMember(dest => dest.Compatibility, opt => opt.Ignore())
                .ForMember(dest => dest.HazardDivision, opt => opt.Ignore())
                .ForMember(dest => dest.BaseItemPrimaryPurposes, opt => opt.Ignore())
                .ForMember(dest => dest.Classification, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.Ignore())
                // Map nullable fields directly - entity now supports nullable values
                .ForMember(dest => dest.IsLinked, opt => opt.MapFrom(src => src.IsLinked ?? false));
        }
    }
}

