using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Versioned Terms & Conditions document.
    /// Only one version is "active" at a time. New versions are inserted; old ones are kept for audit.
    /// </summary>
    public class HelpCenterTermsConditions : FullAuditEntity<long>
    {
        /// <summary>Semantic or sequential version label, e.g. "1.0", "2024-01".</summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>Rich-text / HTML body of the terms document.</summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>Whether this version is the currently active one shown to users.</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>When this version became (or will become) effective.</summary>
        public DateTime EffectiveDate { get; set; }
    }
}
