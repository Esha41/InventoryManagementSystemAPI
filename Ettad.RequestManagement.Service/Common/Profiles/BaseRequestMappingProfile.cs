using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Common.Dtos;

namespace Ettad.RequestManagement.Service.Common.Profiles
{
    public class BaseRequestMappingProfile : AutoMapper.Profile
    {
        public BaseRequestMappingProfile()
        {
            CreateMap<BaseRequest, BaseRequestDto>();
            CreateMap<RequestItem, RequestItemDto>();
        }
    }
}
