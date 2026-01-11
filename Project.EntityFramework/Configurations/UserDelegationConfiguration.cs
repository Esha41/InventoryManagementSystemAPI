using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class UserDelegationConfiguration : IEntityTypeConfiguration<UserDelegation>
    {
        public void Configure(EntityTypeBuilder<UserDelegation> builder)
        {
            builder.HasOne(d => d.DelegatorUser)
                .WithMany()
                .HasForeignKey(d => d.DelegatorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.DelegateeUser)
                .WithMany()
                .HasForeignKey(d => d.DelegateeUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
