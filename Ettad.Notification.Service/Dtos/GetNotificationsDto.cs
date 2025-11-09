namespace Ettad.Notification.Service.Dtos
{
    public class GetNotificationsDto
    {
        public bool? IsRead { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}

