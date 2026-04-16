using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Contact-Us message submitted through the Help Center page.
    /// </summary>
    public class HelpCenterContactMessage : FullAuditEntity<long>
    {
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        /// <summary>Tracks admin read/unread state.</summary>
        public bool IsRead { get; set; }

        /// <summary>Optional admin reply stored alongside the original message.</summary>
        public string? AdminReply { get; set; }

        public DateTime? RepliedAt { get; set; }
        public string? RepliedBy { get; set; }
    }
}
