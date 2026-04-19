using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Represents a help center article (FAQ, guide, or any rich-text content block).
    /// Files (PDFs, docs) are linked via the FileUpload module using FileEntityType.HelpCenter.
    /// </summary>
    public class HelpCenterArticle : FullAuditEntity<long>
    {
        /// <summary>Display title of the article.</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Rich-text or HTML body content.</summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>Optional category / section label (e.g. "Getting Started", "FAQ").</summary>
        public string? Category { get; set; }

        /// <summary>Controls sort order within the same category.</summary>
        public int SortOrder { get; set; }

        /// <summary>Whether the article is publicly visible to users.</summary>
        public bool IsPublished { get; set; } = true;
    }
}
