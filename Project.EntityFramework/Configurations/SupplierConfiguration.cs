using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Suppliers");

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

            // Seed data
            builder.HasData(
                new Supplier
                {
                    Id = 1,
                    NameAr = "شركة الإمدادات العسكرية المتقدمة",
                    NameEn = "Advanced Military Supplies Co.",
                    IsDeleted = false
                },
                new Supplier
                {
                    Id = 2,
                    NameAr = "المؤسسة العامة للتسليح",
                    NameEn = "General Armament Corporation",
                    IsDeleted = false
                },
                new Supplier
                {
                    Id = 3,
                    NameAr = "شركة الصناعات الدفاعية",
                    NameEn = "Defense Industries Company",
                    IsDeleted = false
                },
                new Supplier
                {
                    Id = 4,
                    NameAr = "مجموعة التجهيزات العسكرية",
                    NameEn = "Military Equipment Group",
                    IsDeleted = false
                },
                new Supplier
                {
                    Id = 5,
                    NameAr = "شركة التوريدات الاستراتيجية",
                    NameEn = "Strategic Supplies Corporation",
                    IsDeleted = false
                }
            );
        }
    }
}
