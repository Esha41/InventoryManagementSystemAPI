using Ettad.Data.Entities;
using Ettad.Data.Entities.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class OrderItemHistoryConfiguration : IEntityTypeConfiguration<OrderItemHistory>
    {
        public void Configure(EntityTypeBuilder<OrderItemHistory> builder)
        {
            builder.ToTable("OrderItemHistory");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderId)
                .IsRequired();

            builder.Property(x => x.RequestItemId)
                .IsRequired(false);

            builder.Property(x => x.ItemId)
                .IsRequired();

            builder.Property(x => x.ActionType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.ActionDate)
                .IsRequired();

            builder.Property(x => x.OrderStatus)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.PreviousQuantity)
                .IsRequired(false);

            builder.Property(x => x.NewQuantity)
                .IsRequired(false);

            builder.Property(x => x.ApprovedQuantity)
                .IsRequired(false);

            builder.Property(x => x.SuppliedQuantity)
                .IsRequired(false);

            builder.Property(x => x.DepartmentId)
                .IsRequired();

            builder.Property(x => x.ModifiedByUserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(x => x.ModifiedByUserName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.WorkflowApprovalStepId)
                .IsRequired(false);

            builder.Property(x => x.WorkflowStepId)
                .IsRequired(false);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(2000);

            builder.Property(x => x.SupplyId)
                .IsRequired(false);

            builder.Property(x => x.AssetSupplyId)
                .IsRequired(false);

            builder.Property(x => x.SupplyDetailId)
                .IsRequired(false);

            builder.Property(x => x.AssetSupplyDetailId)
                .IsRequired(false);

            // Indexes for querying history
            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.RequestItemId);
            builder.HasIndex(x => x.ItemId);
            builder.HasIndex(x => x.ActionDate);
            builder.HasIndex(x => x.ActionType);
            builder.HasIndex(x => x.WorkflowApprovalStepId);
            builder.HasIndex(x => new { x.OrderId, x.ItemId }); // Composite index for item history queries

            // Relationships
            builder.HasOne(x => x.Order)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RequestItem)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.RequestItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Item)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ModifiedByUser)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.ModifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.WorkflowApprovalStep)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.WorkflowApprovalStepId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.WorkflowStep)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.WorkflowStepId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Supply)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.SupplyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssetSupply)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.AssetSupplyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SupplyDetail)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.SupplyDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AssetSupplyDetail)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.AssetSupplyDetailId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
