using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("Employees");

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Email)
            .IsRequired(false)
            .HasMaxLength(255);

        builder.Property(x => x.Phone)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.HasOne(x => x.Rank)
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(x => x.RankId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
