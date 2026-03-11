using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.ToTable("Announcements");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Message)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.Priority)
                .IsRequired();

            builder.Property(a => a.DeliveryType)
                .IsRequired()
                .HasDefaultValue(Ettad.Data.Enums.AnnouncementDeliveryType.Banner);

            builder.Property(a => a.IsDismissable)
                .IsRequired();

            builder.Property(a => a.StartDate)
                .IsRequired();

            builder.Property(a => a.EndDate)
                .IsRequired(false);

            builder.Property(a => a.TargetRoles)
                .HasMaxLength(1000);

            builder.Property(a => a.IsActive)
                .IsRequired();

            // Navigation properties
            builder.HasMany(a => a.Dismissals)
                .WithOne(d => d.Announcement)
                .HasForeignKey(d => d.AnnouncementId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => a.IsActive);
            builder.HasIndex(a => a.StartDate);
            builder.HasIndex(a => a.EndDate);
        }
    }
}
