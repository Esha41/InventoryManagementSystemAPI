using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AssetHistoryConfiguration : IEntityTypeConfiguration<AssetHistory>
    {
        public void Configure(EntityTypeBuilder<AssetHistory> builder)
        {
            builder.ToTable("AssetHistory");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ActionType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.ActionDate)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.PreviousStatus)
                .IsRequired(false)
                .HasConversion<int>();

            builder.Property(x => x.NewStatus)
                .IsRequired(false)
                .HasConversion<int>();

            builder.Property(x => x.PreviousLocation)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.NewLocation)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.PerformedByUserId)
                .IsRequired(false)
                .HasMaxLength(450);

            builder.Property(x => x.PerformedByUserName)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(2000);

            builder.Property(x => x.Metadata)
                .IsRequired(false);

            // Indexes for querying history
            builder.HasIndex(x => x.AssetId);
            builder.HasIndex(x => x.ActionDate);
            builder.HasIndex(x => x.ActionType);
            builder.HasIndex(x => x.OrderId);

            // Relationships
            builder.HasOne(x => x.Asset)
                .WithMany(a => a.History)
                .IsRequired()
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Order)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssetSupply)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.AssetSupplyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssetAssignment)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.AssetAssignmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PreviousDepartment)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.PreviousDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.NewDepartment)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.NewDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PreviousCustodian)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.PreviousCustodianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.NewCustodian)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.NewCustodianId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

