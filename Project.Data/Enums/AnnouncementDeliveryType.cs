using System;

namespace Ettad.Data.Enums
{
    [Flags]
    public enum AnnouncementDeliveryType
    {
        Banner = 1,
        Notification = 2,
        Both = Banner | Notification
    }
}
