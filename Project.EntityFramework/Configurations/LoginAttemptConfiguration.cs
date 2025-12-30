using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class LoginAttemptConfiguration : IEntityTypeConfiguration<LoginAttempt>
    {
        public void Configure(EntityTypeBuilder<LoginAttempt> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("LoginAttempts");

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.UserId)
                .HasMaxLength(450); // Match AspNetUsers.Id length

            builder.Property(x => x.IsSuccessful)
                .IsRequired();

            builder.Property(x => x.FailureReason)
                .HasMaxLength(500);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45); // IPv6 max length

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500);

            builder.Property(x => x.LoginType)
                .IsRequired();

            builder.Property(x => x.AttemptDate)
                .IsRequired();

            builder.HasIndex(x => x.Username);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.AttemptDate);
            builder.HasIndex(x => new { x.Username, x.AttemptDate });

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
