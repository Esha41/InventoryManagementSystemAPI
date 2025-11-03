using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Departments");

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameAr)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameEn)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Seed data
            builder.HasData(
                new Department
                {
                    Id = 1,
                    Code = "LOG",
                    NameAr = "قسم اللوجستيات",
                    NameEn = "Logistics Department",
                    IsDeleted = false
                },
                new Department
                {
                    Id = 2,
                    Code = "OPS",
                    NameAr = "قسم العمليات",
                    NameEn = "Operations Department",
                    IsDeleted = false
                },
                new Department
                {
                    Id = 3,
                    Code = "INV",
                    NameAr = "قسم المخزون",
                    NameEn = "Inventory Department",
                    IsDeleted = false
                },
                new Department
                {
                    Id = 4,
                    Code = "ARM",
                    NameAr = "قسم التسليح",
                    NameEn = "Armament Department",
                    IsDeleted = false
                },
                new Department
                {
                    Id = 5,
                    Code = "MNT",
                    NameAr = "قسم الصيانة",
                    NameEn = "Maintenance Department",
                    IsDeleted = false
                }
            );
        }
    }
}
