using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class FileUplodMasterConfiguration : IEntityTypeConfiguration<FileUplodMaster>
    {
        public void Configure(EntityTypeBuilder<FileUplodMaster> builder)
        {
            builder.ToTable("FileUplodMaster");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.OriginalName)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.IsMain)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasMany(x => x.Details)
                .WithOne(d => d.FileUplodMaster)
                .HasForeignKey(d => d.FileUplodMasterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


