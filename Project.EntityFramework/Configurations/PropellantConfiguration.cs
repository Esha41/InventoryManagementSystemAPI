using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class PropellantConfiguration : IEntityTypeConfiguration<Propellant>
    {
        public void Configure(EntityTypeBuilder<Propellant> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Propellants");

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

            // Configure CreationDate to use database default - prevents EF Core from comparing it in seeded data
            builder.Property(x => x.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            // Seed data
            builder.HasData(
                new Propellant
                {
                    Id = 1,
                    NameAr = "بارود أحادي القاعدة",
                    NameEn = "Single-base Powder",
                    IsDeleted = false
                },
                new Propellant
                {
                    Id = 2,
                    NameAr = "بارود ثنائي القاعدة",
                    NameEn = "Double-base Powder",
                    IsDeleted = false
                },
                new Propellant
                {
                    Id = 3,
                    NameAr = "بارود ثلاثي القاعدة",
                    NameEn = "Triple-base Powder",
                    IsDeleted = false
                },
                new Propellant
                {
                    Id = 4,
                    NameAr = "نيتروسليلوز",
                    NameEn = "Nitrocellulose",
                    IsDeleted = false
                },
                new Propellant
                {
                    Id = 5,
                    NameAr = "كورديت",
                    NameEn = "Cordite",
                    IsDeleted = false
                }
            );
        }
    }
}
