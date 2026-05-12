namespace Ettad.HelpCenter.Service.Dtos
{
    // ─── Article ────────────────────────────────────────────────────────────────

    public class HelpCenterArticleDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Category { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModifiedBy { get; set; }
    }

    public class CreateHelpCenterArticleDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Category { get; set; }
        public bool IsPublished { get; set; } = true;
    }

    public class UpdateHelpCenterArticleDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Category { get; set; }
        public bool? IsPublished { get; set; }
    }

    // ─── Contact Message ────────────────────────────────────────────────────────

    public class HelpCenterContactMessageDto
    {
        public long Id { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public string? AdminReply { get; set; }
        public DateTime? RepliedAt { get; set; }
        public string? RepliedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class SubmitContactMessageDto
    {
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    public class ReplyContactMessageDto
    {
        public string AdminReply { get; set; } = string.Empty;
    }

    /// <summary>Support email and phone displayed on the Contact Us tab (managed in admin).</summary>
    public class HelpCenterContactDisplayDto
    {
        public string SupportEmail { get; set; } = string.Empty;
        public string SupportPhone { get; set; } = string.Empty;
    }

    public class UpdateHelpCenterContactDisplayDto
    {
        public string SupportEmail { get; set; } = string.Empty;
        public string SupportPhone { get; set; } = string.Empty;
    }

    // ─── Terms & Conditions ─────────────────────────────────────────────────────

    public class HelpCenterTermsDto
    {
        public long Id { get; set; }
        public string Version { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class UpsertHelpCenterTermsDto
    {
        public string Version { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
    }

    public class UpdateHelpCenterTermsDto
    {
        public string? Version { get; set; }
        public string? Content { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }

    /// <summary>Whether the user must accept (acknowledge) the current active terms before using the app; true whenever active terms exist so the gate runs on each login.</summary>
    public class TermsAcceptanceStatusDto
    {
        public bool MustAccept { get; set; }
        public HelpCenterTermsDto? Terms { get; set; }
    }
}
