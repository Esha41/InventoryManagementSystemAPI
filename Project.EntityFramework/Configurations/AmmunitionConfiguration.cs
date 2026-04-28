using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AmmunitionConfiguration : IEntityTypeConfiguration<Ammunition>
    {
        public void Configure(EntityTypeBuilder<Ammunition> builder)
        {
            builder.ToTable("Ammunitions");

            builder.Property(x => x.AmmunitionType)
                .IsRequired();

            builder.Property(x => x.BulletDiameter)
                .IsRequired(false);

            builder.Property(x => x.ArmNumber)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.HasOne(x => x.LookupCaliber)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CaliberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.CaliberId);

            builder.HasOne(x => x.NatureOption)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.NatureOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProjectileColor)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ProjectileColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProjectailMaterial)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ProjectailMaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CaseType)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CaseTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Propellant)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.PropellantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Compatibility)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CompatibilityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.HazardDivision)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.HazardDivisionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BulletDiameterUnit)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.BulletDiameterUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure CreationDate to use database default - prevents EF Core from comparing it in seeded data
            builder.Property(x => x.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
