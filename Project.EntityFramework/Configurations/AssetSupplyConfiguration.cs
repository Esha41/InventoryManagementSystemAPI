using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AssetSupplyConfiguration : IEntityTypeConfiguration<AssetSupply>
    {
        public void Configure(EntityTypeBuilder<AssetSupply> builder)
        {
            builder.ToTable("AssetSupplies");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SupplyDate)
                .IsRequired(false);

            builder.Property(x => x.SubmissionStatus)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(SupplySubmissionStatus.Submitted);

            builder.Property(x => x.FulfillmentStatus)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(SupplyFulfillmentStatus.Partial);

            builder.Property(x => x.ReceiverName)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(x => x.ReceiverMilitaryId)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.Location)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.ExpectedReturnDate)
                .IsRequired(false);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(2000);

            builder.Property(x => x.CustodianId)
                .IsRequired()
                .HasMaxLength(450);

            // Indexes
            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.SubmissionStatus);
            builder.HasIndex(x => x.DepartmentId);

            // Relationships
            builder.HasOne(x => x.Order)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Custodian)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.CustodianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ReceiverRank)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ReceiverRankId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
