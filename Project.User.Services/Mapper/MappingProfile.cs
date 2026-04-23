using AutoMapper;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Entities;
using Ettad.User.Services.DTO;
using Ettad.User.Services.DTO.AuthenticationDto;
namespace Ettad.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
         
            CreateMap<RegisterUserDto, ApplicationUser>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<ApplicationUser, GetUserDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName)).ReverseMap();


            // Maps the Identity Role to our DTO
            CreateMap<ApplicationRole, RoleDto>()
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsDefaultRole, opt => opt.MapFrom(src => src.IsDefaultRole ?? false));

            // Maps the Create DTO to the Identity Role
            CreateMap<CreateRoleDto, ApplicationRole>();
         

        }
    }
}
