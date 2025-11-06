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
    internal class RankConfiguration : IEntityTypeConfiguration<Rank>
    {

        public void Configure(EntityTypeBuilder<Rank> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Ranks");

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
                new Rank
                {
                    Id = 1,
                    NameAr = "عقيد",
                    NameEn = "Colonel",
                    IsDeleted = false
                },
                new Rank
                {
                    Id = 2,
                    NameAr = "مقدم",
                    NameEn = "Lieutenant Colonel",
                    IsDeleted = false
                },
                new Rank
                {
                    Id = 3,
                    NameAr = "رائد",
                    NameEn = "Major",
                    IsDeleted = false
                },
                new Rank
                {
                    Id = 4,
                    NameAr = "نقيب",
                    NameEn = "Captain",
                    IsDeleted = false
                },
                new Rank
                {
                    Id = 5,
                    NameAr = "ملازم أول",
                    NameEn = "First Lieutenant",
                    IsDeleted = false
                },
                new Rank
                {
                    Id = 6,
                    NameAr = "ملازم",
                    NameEn = "Second Lieutenant",
                    IsDeleted = false
                },
                new Rank
                {
                    Id = 7,
                    NameAr = "رقيب أول",
                    NameEn = "Master Sergeant",
                    IsDeleted = false
                },
                new Rank
                {
                    Id = 8,
                    NameAr = "رقيب",
                    NameEn = "Sergeant",
                    IsDeleted = false
                },
                new Rank
                {
                    Id = 9,
                    NameAr = "عريف",
                    NameEn = "Corporal",
                    IsDeleted = false
                },
                new Rank
                {
                    Id = 10,
                    NameAr = "جندي",
                    NameEn = "Private",
                    IsDeleted = false
                }
            );
        }
    }
}
