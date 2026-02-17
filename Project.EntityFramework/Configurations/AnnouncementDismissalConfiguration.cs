using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class AnnouncementDismissalConfiguration : IEntityTypeConfiguration<AnnouncementDismissal>
    {
        public void Configure(EntityTypeBuilder<AnnouncementDismissal> builder)
        {
            builder.ToTable("AnnouncementDismissals");

            builder.HasKey(ad => ad.Id);

            builder.Property(ad => ad.AnnouncementId)
                .IsRequired();

            builder.Property(ad => ad.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(ad => ad.DismissedAt)
                .IsRequired();

            // Navigation properties
            builder.HasOne(ad => ad.Announcement)
                .WithMany(a => a.Dismissals)
                .HasForeignKey(ad => ad.AnnouncementId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ad => ad.User)
                .WithMany()
                .HasForeignKey(ad => ad.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(ad => ad.UserId);
            builder.HasIndex(ad => new { ad.AnnouncementId, ad.UserId })
                .IsUnique();
        }
    }
}
