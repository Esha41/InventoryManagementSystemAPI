using AutoMapper;
using Ettad.Announcement.Service.Dtos;
using AnnouncementEntity = Ettad.Data.Entities.Announcement;

namespace Ettad.Announcement.Service.Mapper
{
    public class AnnouncementMappingProfile : Profile
    {
        public AnnouncementMappingProfile()
        {
            CreateMap<AnnouncementEntity, AnnouncementDto>();
            CreateMap<AnnouncementEntity, ActiveAnnouncementDto>();
        }
    }
}
