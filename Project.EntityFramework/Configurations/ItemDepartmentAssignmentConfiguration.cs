using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class ItemDepartmentAssignmentConfiguration : IEntityTypeConfiguration<ItemDepartmentAssignment>
    {
        public void Configure(EntityTypeBuilder<ItemDepartmentAssignment> builder)
        {
            builder.ToTable("ItemDepartmentAssignments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(1000);

            // Unique constraint: ItemId + DepartmentId combination must be unique
            builder.HasIndex(x => new { x.ItemId, x.DepartmentId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Indexes for performance
            builder.HasIndex(x => x.ItemId);
            builder.HasIndex(x => x.DepartmentId);

            // Relationships
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

            // Configure CreationDate to use database default
            builder.Property(x => x.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
