using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class HelpCenterArticleConfiguration : IEntityTypeConfiguration<HelpCenterArticle>
    {
        public void Configure(EntityTypeBuilder<HelpCenterArticle> builder)
        {
            builder.ToTable("HelpCenterArticles");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(a => a.Content)
                .IsRequired();

            builder.Property(a => a.Category)
                .HasMaxLength(100);

            builder.Property(a => a.SortOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(a => a.IsPublished)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(a => a.IsPublished);
            builder.HasIndex(a => a.Category);
            builder.HasIndex(a => a.SortOrder);
        }
    }
}
