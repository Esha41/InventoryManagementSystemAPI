using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Batches.Dtos;

namespace Ettad.Inventory.Service.Batches.Profiles
{
    public class BatchMappingProfile : Profile
    {
        public BatchMappingProfile()
        {
            CreateMap<Batch, BatchDto>()
                .ForMember(dest => dest.Depot, opt => opt.MapFrom(src => src.Depot != null ? src.Depot : null))
                .ForMember(dest => dest.Assets, opt => opt.Ignore())
                .ForMember(dest => dest.AssetCount, opt => opt.Ignore());
        }
    }
}
