using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class NotificationReceiverConfiguration : IEntityTypeConfiguration<NotificationReceiver>
    {
        public void Configure(EntityTypeBuilder<NotificationReceiver> builder)
        {
            builder.ToTable("NotificationReceivers");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NotificationId)
                .IsRequired();

            builder.Property(x => x.IsRead)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.ReadAt);

            // Relationship to Notification
            builder.HasOne(x => x.Notification)
                .WithMany(x => x.Receivers)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship to ApplicationUser (optional)
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasIndex(x => x.NotificationId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.IsRead);
        }
    }
}

