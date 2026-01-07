using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class BlacklistedTokenConfiguration : IEntityTypeConfiguration<BlacklistedToken>
    {
        public void Configure(EntityTypeBuilder<BlacklistedToken> builder)
        {
            builder.ToTable("BlacklistedTokens");

            builder.HasKey(bt => bt.Id);

            builder.Property(bt => bt.TokenId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(bt => bt.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(bt => bt.BlacklistedAt)
                .IsRequired();

            builder.Property(bt => bt.ExpiresAt)
                .IsRequired();

            builder.Property(bt => bt.Reason)
                .HasMaxLength(500);

            // Create unique index on TokenId for fast lookups
            builder.HasIndex(bt => bt.TokenId)
                .IsUnique()
                .HasDatabaseName("IX_BlacklistedTokens_TokenId");

            // Create index on UserId for querying tokens by user
            builder.HasIndex(bt => bt.UserId)
                .HasDatabaseName("IX_BlacklistedTokens_UserId");

            // Create index on ExpiresAt for efficient cleanup queries
            builder.HasIndex(bt => bt.ExpiresAt)
                .HasDatabaseName("IX_BlacklistedTokens_ExpiresAt");
        }
    }
}

