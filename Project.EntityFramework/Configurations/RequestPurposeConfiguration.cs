using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class RequestPurposeConfiguration : IEntityTypeConfiguration<RequestPurpose>
    {
        public void Configure(EntityTypeBuilder<RequestPurpose> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("RequestPurposes");

            builder.Property(x => x.RequestType)
                .IsRequired();

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameAr)
                .IsUnique();

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameEn)
                .IsUnique();

            // Seed data
            builder.HasData(
                // Order purposes
                new RequestPurpose
                {
                    Id = 1,
                    NameAr = "طلب عادي",
                    NameEn = "Normal Order",
                    RequestType = RequestType.Order
                },
                new RequestPurpose
                {
                    Id = 2,
                    NameAr = "طلب خدمة",
                    NameEn = "Duty Order",
                    RequestType = RequestType.Order
                },
                new RequestPurpose
                {
                    Id = 3,
                    NameAr = "طلب عملية",
                    NameEn = "Operation Order",
                    RequestType = RequestType.Order
                },
                new RequestPurpose
                {
                    Id = 4,
                    NameAr = "طلب تدريبي",
                    NameEn = "Training Order",
                    RequestType = RequestType.Order
                },
                new RequestPurpose
                {
                    Id = 12,
                    NameAr = "تطهير الميدان",
                    NameEn = "Field Clearance",
                    RequestType = RequestType.Order
                },
                // Return purposes
                new RequestPurpose
                {
                    Id = 5,
                    NameAr = "إرجاع عادي",
                    NameEn = "Normal Return",
                    RequestType = RequestType.Return
                },
                new RequestPurpose
                {
                    Id = 6,
                    NameAr = "إرجاع بعد انتهاء الخدمة",
                    NameEn = "Return After Service",
                    RequestType = RequestType.Return
                },
                new RequestPurpose
                {
                    Id = 7,
                    NameAr = "إرجاع بعد العملية",
                    NameEn = "Return After Operation",
                    RequestType = RequestType.Return
                },
                new RequestPurpose
                {
                    Id = 8,
                    NameAr = "إرجاع بعد التدريب",
                    NameEn = "Return After Training",
                    RequestType = RequestType.Return
                },
                // Discard purposes
                new RequestPurpose
                {
                    Id = 9,
                    NameAr = "تسديد تالف",
                    NameEn = "Damaged Discard",
                    RequestType = RequestType.Discard
                },
                new RequestPurpose
                {
                    Id = 10,
                    NameAr = "تسديد منتهي الصلاحية",
                    NameEn = "Expired Discard",
                    RequestType = RequestType.Discard
                },
                new RequestPurpose
                {
                    Id = 11,
                    NameAr = "تسديد غير مستخدم",
                    NameEn = "Unused Discard",
                    RequestType = RequestType.Discard
                }
            );
        }
    }
}
