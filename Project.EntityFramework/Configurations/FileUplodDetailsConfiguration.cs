using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class FileUplodDetailsConfiguration : IEntityTypeConfiguration<FileUplodDetails>
    {
        public void Configure(EntityTypeBuilder<FileUplodDetails> builder)
        {
            builder.ToTable("FileUplodDetails");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Entity)
                .HasColumnName("Entity")
                .IsRequired();

            builder.Property(x => x.EntityId)
                .HasColumnName("entityId")
                .IsRequired();

            builder.Property(x => x.AttachmentRequirementId)
                .HasColumnName("AttachmentRequirementId")
                .IsRequired(false);

            builder.HasOne(x => x.AttachmentRequirement)
                .WithMany()
                .HasForeignKey(x => x.AttachmentRequirementId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


