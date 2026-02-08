using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class BaseItemConfiguration : IEntityTypeConfiguration<BaseItem>
    {
        public void Configure(EntityTypeBuilder<BaseItem> builder)
        {
            builder.ToTable("BaseItems");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ItemNo)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.ItemNo)
                .IsUnique();

            builder.Property(x => x.ItemType)
                .IsRequired();

            builder.Property(x => x.PartNo)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.Price)
                .IsRequired(false)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.MinimumQuantity)
                .IsRequired(false);

            builder.Property(x => x.Distribution)
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.Property(x => x.ReferenceNo)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.UNNumber)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(5000);

            builder.Property(x => x.ClassificationId)
                .IsRequired(false);

            builder.Property(x => x.TypeId)
                .IsRequired(false);

            builder.HasOne(x => x.Classification)
                .WithMany()
                .HasForeignKey(x => x.ClassificationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Type)
                .WithMany()
                .HasForeignKey(x => x.TypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Performance indexes for pagination, filtering, and sorting
            // Note: Foreign key relationships don't automatically create indexes
            // We need explicit indexes for query performance
            builder.HasIndex(x => x.IsDeleted)
                .HasFilter("[IsDeleted] = 0");

           
            builder.HasIndex(x => x.Name);

           
            builder.HasIndex(x => x.TypeId);
            builder.HasIndex(x => x.ClassificationId);

            builder.HasIndex(x => new { x.TypeId, x.IsDeleted })
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(x => new { x.ClassificationId, x.IsDeleted })
                .HasFilter("[IsDeleted] = 0");
        }
    }
}
