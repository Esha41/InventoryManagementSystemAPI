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

            // Configure CreationDate to use database default - prevents EF Core from comparing it in seeded data
            builder.Property(x => x.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            builder.HasData(
                new Department
                {
                    Id = 1,
                    Code = "QELF",
                    NameAr = "القوات البرية الأميرية القطرية",
                    NameEn = "Qatar Emiri Land Forces",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 2,
                    Code = "QEAF",
                    NameAr = "القوات الجوية الأميرية القطرية",
                    NameEn = "Qatar Emiri Air Force",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 3,
                    Code = "QENF",
                    NameAr = "القوات البحرية الأميرية القطرية",
                    NameEn = "Qatar Emiri Navy",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 4,
                    Code = "EGD",
                    NameAr = "الحرس الأميري",
                    NameEn = "Emiri Guard Directorate",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 5,
                    Code = "JSFC",
                    NameAr = "قيادة القوات الخاصة المشتركة",
                    NameEn = "Joint Special Forces Command",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 6,
                    Code = "NSAC",
                    NameAr = "أكاديمية الخدمة الوطنية",
                    NameEn = "National Service Academy",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 7,
                    Code = "MID",
                    NameAr = "مديرية الاستخبارات العسكرية",
                    NameEn = "Military Intelligence Directorate",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 8,
                    Code = "LOGC",
                    NameAr = "قيادة الإمداد والتموين",
                    NameEn = "Logistics and Supply Command",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 9,
                    Code = "MP",
                    NameAr = "قيادة الشرطة العسكرية",
                    NameEn = "Military Police Command",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 10,
                    Code = "MED",
                    NameAr = "الخدمات الطبية للقوات المسلحة",
                    NameEn = "Armed Forces Medical Services",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 11,
                    Code = "TRAD",
                    NameAr = "قيادة التدريب والعقيدة",
                    NameEn = "Training and Doctrine Command",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 12,
                    Code = "ADC",
                    NameAr = "قيادة الدفاع الجوي",
                    NameEn = "Air Defense Command",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 13,
                    Code = "CYBC",
                    NameAr = "قيادة الاتصالات والدفاع السيبراني",
                    NameEn = "Cyber Defense & Communications Command",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 14,
                    Code = "MT",
                    NameAr = "مديرية التدريب العسكري",
                    NameEn = "Military Training",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 15,
                    Code = "DoA",
                    NameAr = "مديرية التسليح",
                    NameEn = "Directorate of Armament",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 16,
                    Code = "MO",
                    NameAr = "هئية العمليات",
                    NameEn = "Military Operation",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 17,
                    Code = "CoS",
                    NameAr = "مكتب رئيس الأركان",
                    NameEn = "Chief of Staff Office",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                },
                new Department
                {
                    Id = 18,
                    Code = "Inventory",
                    NameAr = "مستودعات الأسلحة والذخيرة المركزيه",
                    NameEn = "Inventory",
                    IsDeleted = false,
                    CreatedBy = "SYSTEM"
                }
            );
        }
    }
}
