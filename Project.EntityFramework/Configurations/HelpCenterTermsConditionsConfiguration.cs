using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class HelpCenterTermsConditionsConfiguration : IEntityTypeConfiguration<HelpCenterTermsConditions>
    {
        public void Configure(EntityTypeBuilder<HelpCenterTermsConditions> builder)
        {
            builder.ToTable("HelpCenterTermsConditions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Version)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Content)
                .IsRequired();

            builder.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(t => t.EffectiveDate)
                .IsRequired();

            builder.HasIndex(t => t.IsActive);
            builder.HasIndex(t => t.Version).IsUnique();
            builder.HasIndex(t => t.EffectiveDate);
        }
    }
}
