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

            builder.Property(x => x.OrderNo)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(x => x.OrderNo)
                .IsUnique();

            builder.Property(x => x.Reason)
                .IsRequired(false);

            builder.Property(x => x.Notes)
                .IsRequired(false);

            // ✅ Convert enums to string in DB
            builder.Property(x => x.RequestType)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Priority)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(255);

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

            builder.HasOne(x => x.Reciever)
                .WithMany()
                .HasForeignKey(x => x.RecieverId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Depot)
                .WithMany()
                .HasForeignKey(x => x.DepotId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
