using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.EntityFramework.Configurations
{
    internal class ApplicationEntityConfiguration : IEntityTypeConfiguration<ApplicationEntity>
    {
        public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("ApplicationEntity");

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
            builder.HasData(
                new ApplicationEntity
                {
                    Id = 1,
                    Code = "ORE",
                    NameAr = "الجهة الطالبة للطلب",
                    NameEn = "Order Requesting Entity",
                    IsDeleted = false
                },
                new ApplicationEntity
                {
                    Id = 2,
                    Code = "MT",
                    NameAr = "التدريب العسكري",
                    NameEn = "Military Training",
                    IsDeleted = false
                },
                new ApplicationEntity
                {
                    Id = 3,
                    Code = "DoA",
                    NameAr = "مدير التسليح",
                    NameEn = "Director of Armament",
                    IsDeleted = false
                },
                new ApplicationEntity
                {
                    Id = 4,
                    Code = "MO",
                    NameAr = "العمليات العسكرية",
                    NameEn = "Military Operations",
                    IsDeleted = false
                },
                new ApplicationEntity
                {
                    Id = 5,
                    Code = "DCoS",
                    NameAr = "نائب رئيس الأركان",
                    NameEn = "Deputy Chief of Staff",
                    IsDeleted = false
                },
                new ApplicationEntity
                {
                    Id = 6,
                    Code = "Function",
                    NameAr = "وظيفة",
                    NameEn = "Function",
                    IsDeleted = false
                },
                new ApplicationEntity
                {
                    Id = 7,
                    Code = "CoS",
                    NameAr = "رئيس الأركان",
                    NameEn = "Chief of Staff",
                    IsDeleted = false
                },
                new ApplicationEntity
                {
                    Id = 8,
                    Code = "Inventory",
                    NameAr = "المخزون",
                    NameEn = "Inventory",
                    IsDeleted = false
                }              
                
            );
        }

    }
}
