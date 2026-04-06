using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AssetAssignmentConfiguration : IEntityTypeConfiguration<AssetAssignment>
    {
        public void Configure(EntityTypeBuilder<AssetAssignment> builder)
        {
            builder.ToTable("AssetAssignments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AssignDate)
                .IsRequired();

            builder.Property(x => x.ExpectedReturnDate)
                .IsRequired(false);

            builder.Property(x => x.ActualReturnDate)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(AssetAssignmentStatus.Active);

            builder.Property(x => x.Purpose)
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(2000);

            builder.Property(x => x.Location)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.ConditionOnAssign)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.ConditionOnReturn)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.ReceiverName)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(x => x.ReceiverMilitaryId)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.CustodianId)
                .IsRequired(false);

            // Indexes for performance
            builder.HasIndex(x => new { x.AssetId, x.Status });
            builder.HasIndex(x => x.DepartmentId);
            builder.HasIndex(x => x.CustodianId);
            builder.HasIndex(x => x.OrderId);

            // Relationships
            builder.HasOne(x => x.Asset)
                .WithMany(a => a.Assignments)
                .IsRequired()
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Order)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssetSupply)
                .WithMany(s => s.Assignments)
                .IsRequired(false)
                .HasForeignKey(x => x.AssetSupplyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Custodian)
                .WithMany()
                .IsRequired(false)
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

