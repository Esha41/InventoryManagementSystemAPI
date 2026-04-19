using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class HelpCenterContactMessageConfiguration : IEntityTypeConfiguration<HelpCenterContactMessage>
    {
        public void Configure(EntityTypeBuilder<HelpCenterContactMessage> builder)
        {
            builder.ToTable("HelpCenterContactMessages");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.SenderName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(m => m.SenderEmail)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Subject)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(m => m.Body)
                .IsRequired();

            builder.Property(m => m.IsRead)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(m => m.AdminReply)
                .IsRequired(false);

            builder.Property(m => m.RepliedAt)
                .IsRequired(false);

            builder.Property(m => m.RepliedBy)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.HasIndex(m => m.IsRead);
            builder.HasIndex(m => m.CreationDate);
        }
    }
}
