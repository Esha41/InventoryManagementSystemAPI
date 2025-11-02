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
