using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class BaseRequestConfiguration : IEntityTypeConfiguration<BaseRequest>
    {
        public void Configure(EntityTypeBuilder<BaseRequest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("BaseRequests");

            builder.Property(x => x.RequestNo)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(x => x.RequestNo)
                .IsUnique();

            builder.Property(x => x.Reason)
                .IsRequired(false);

            builder.Property(x => x.Notes)
                .IsRequired(false);

            // Enums stored as int by default
            builder.Property(x => x.RequestType)
                .IsRequired();

            builder.Property(x => x.Priority)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.RequesterId)
                .IsRequired(false)
                .HasMaxLength(450); // Match AspNetUsers.Id length

            builder.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RequestPurpose)
                .WithMany()
                .HasForeignKey(x => x.RequestPurposeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Requester)
                .WithMany()
                .HasForeignKey(x => x.RequesterId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
