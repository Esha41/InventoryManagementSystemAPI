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
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);


            builder.Property(x => x.Email)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(x => x.Phone)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.HasOne(x => x.Rank)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.RankId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data
            builder.HasData(
                new Employee
                {
                    Id = 1,
                    NameAr = "أحمد محمد العلي",
                    NameEn = "Ahmed Mohammed Al-Ali",
                    IdNo = "1234567890",
                    Email = "ahmed.ali@example.com",
                    Phone = "+966501234567",
                    Address = "الرياض، المملكة العربية السعودية",
                    Notes = "قائد قسم العمليات",
                    RankId = 1
                },
                new Employee
                {
                    Id = 2,
                    NameAr = "محمد عبدالله السالم",
                    NameEn = "Mohammed Abdullah Al-Salem",
                    IdNo = "1234567891",
                    Email = "mohammed.salem@example.com",
                    Phone = "+966501234568",
                    Address = "جدة، المملكة العربية السعودية",
                    Notes = "مسؤول المخزون",
                    RankId = 2
                },
                new Employee
                {
                    Id = 3,
                    NameAr = "خالد سعد الدوسري",
                    NameEn = "Khalid Saad Al-Dosari",
                    IdNo = "1234567892",
                    Email = "khalid.dosari@example.com",
                    Phone = "+966501234569",
                    Address = "الدمام، المملكة العربية السعودية",
                    Notes = "مشرف اللوجستيات",
                    RankId = 3
                },
                new Employee
                {
                    Id = 4,
                    NameAr = "فهد عبدالرحمن القحطاني",
                    NameEn = "Fahad Abdulrahman Al-Qahtani",
                    IdNo = "1234567893",
                    Email = "fahad.qhtani@example.com",
                    Phone = "+966501234570",
                    Address = "الرياض، المملكة العربية السعودية",
                    Notes = "ضابط العمليات",
                    RankId = 4
                },
                new Employee
                {
                    Id = 5,
                    NameAr = "عبدالله يوسف العتيبي",
                    NameEn = "Abdullah Youssef Al-Otaibi",
                    IdNo = "1234567894",
                    Email = "abdullah.otaibi@example.com",
                    Phone = "+966501234571",
                    Address = "الطائف، المملكة العربية السعودية",
                    Notes = "مساعد إداري",
                    RankId = 5
                },
                new Employee
                {
                    Id = 6,
                    NameAr = "سعد علي الحربي",
                    NameEn = "Saad Ali Al-Harbi",
                    IdNo = "1234567895",
                    Email = "saad.harbi@example.com",
                    Phone = "+966501234572",
                    Address = "الرياض، المملكة العربية السعودية",
                    Notes = "ضابط",
                    RankId = 6
                },
                new Employee
                {
                    Id = 7,
                    NameAr = "عمر حسن الغامدي",
                    NameEn = "Omar Hassan Al-Ghamdi",
                    IdNo = "1234567896",
                    Email = "omar.ghamdi@example.com",
                    Phone = "+966501234573",
                    Address = "جدة، المملكة العربية السعودية",
                    Notes = "رقيب أول",
                    RankId = 7
                },
                new Employee
                {
                    Id = 8,
                    NameAr = "يوسف إبراهيم الزهراني",
                    NameEn = "Youssef Ibrahim Al-Zahrani",
                    IdNo = "1234567897",
                    Email = "youssef.zahrani@example.com",
                    Phone = "+966501234574",
                    Address = "الدمام، المملكة العربية السعودية",
                    Notes = "رقيب",
                    RankId = 8
                },
                new Employee
                {
                    Id = 9,
                    NameAr = "علي محمود الشمري",
                    NameEn = "Ali Mahmoud Al-Shamri",
                    IdNo = "1234567898",
                    Email = "ali.shamri@example.com",
                    Phone = "+966501234575",
                    Address = "الرياض، المملكة العربية السعودية",
                    Notes = "عريف",
                    RankId = 9
                },
                new Employee
                {
                    Id = 10,
                    NameAr = "حسن عبدالعزيز المطيري",
                    NameEn = "Hassan Abdulaziz Al-Mutairi",
                    IdNo = "1234567899",
                    Email = "hassan.mutairi@example.com",
                    Phone = "+966501234576",
                    Address = "جدة، المملكة العربية السعودية",
                    Notes = "جندي",
                    RankId = 10
                }
            );
        }
    }
}
