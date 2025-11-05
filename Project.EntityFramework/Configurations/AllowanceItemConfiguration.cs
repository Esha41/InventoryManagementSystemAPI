using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AllowanceItemConfiguration : IEntityTypeConfiguration<AllowanceItem>
    {
        public void Configure(EntityTypeBuilder<AllowanceItem> builder)
        {
            builder.ToTable("AllowanceItems");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Year)
                .IsRequired();

            // Unique constraint: ItemId + DepartmentId + Year combination must be unique
            builder.HasIndex(x => new { x.ItemId, x.DepartmentId, x.Year })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasOne(x => x.Item)
                .WithMany()
                .IsRequired(true)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                .WithMany()
                .IsRequired(true)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
