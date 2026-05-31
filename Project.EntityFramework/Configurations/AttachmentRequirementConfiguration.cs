using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AttachmentRequirementConfiguration : IEntityTypeConfiguration<AttachmentRequirement>
    {
        public void Configure(EntityTypeBuilder<AttachmentRequirement> builder)
        {
            builder.ToTable("AttachmentRequirements");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ParentType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.ParentId)
                .IsRequired();

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Code)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.ApplicableEntityType)
                .HasConversion<int?>()
                .IsRequired(false);

            builder.Property(x => x.IsRequired)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.MinCount)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(x => x.MaxCount)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasIndex(x => new { x.ParentType, x.ParentId, x.NameEn })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasFilter("[Code] IS NOT NULL");
        }
    }
}
