using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Employees");

            builder.Property(x => x.NameAr)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.NameEn)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.MilitaryId)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.Phone)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(5000);

            builder.HasOne(x => x.Department)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Rank)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.RankId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

