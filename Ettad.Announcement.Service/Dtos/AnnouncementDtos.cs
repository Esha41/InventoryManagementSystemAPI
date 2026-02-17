using Ettad.Data.Enums;

namespace Ettad.Announcement.Service.Dtos
{
    public class AnnouncementDto
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public RequestPriority Priority { get; set; }
        public bool IsDismissable { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? TargetRoles { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class CreateAnnouncementDto
    {
        public string Message { get; set; }
        public RequestPriority Priority { get; set; }
        public bool IsDismissable { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? TargetRoles { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateAnnouncementDto
    {
        public string? Message { get; set; }
        public RequestPriority? Priority { get; set; }
        public bool? IsDismissable { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? TargetRoles { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ActiveAnnouncementDto
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public RequestPriority Priority { get; set; }
        public bool IsDismissable { get; set; }
    }
}
