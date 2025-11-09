using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Notification.Service.Dtos;
using NotificationEntity = Ettad.Data.Entities.Notification;

namespace Ettad.Notification.Service.Mapper
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<NotificationEntity, NotificationDto>()
                .ForMember(dest => dest.IsRead, opt => opt.Ignore())
                .ForMember(dest => dest.ReadAt, opt => opt.Ignore());

            CreateMap<NotificationReceiver, NotificationReceiverDto>();
        }
    }
}
