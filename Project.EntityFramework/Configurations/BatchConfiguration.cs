using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Batches");

            builder.Property(x => x.BatchNumber)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.BatchNumber)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasOne(x => x.Depot)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.DepotId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PrimaryPurpos)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.PrimaryPurposId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Assets)
                .WithOne(x => x.Batch)
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
