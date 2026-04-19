using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class HelpCenterContactDisplaySettingsConfiguration : IEntityTypeConfiguration<HelpCenterContactDisplaySettings>
    {
        public void Configure(EntityTypeBuilder<HelpCenterContactDisplaySettings> builder)
        {
            builder.ToTable("HelpCenterContactDisplaySettings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.SupportEmail)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.SupportPhone)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ModifiedBy)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.HasData(new HelpCenterContactDisplaySettings
            {
                Id = 1,
                SupportEmail = string.Empty,
                SupportPhone = string.Empty
            });
        }
    }
}
