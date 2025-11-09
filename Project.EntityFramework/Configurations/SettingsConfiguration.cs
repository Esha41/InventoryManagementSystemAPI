using Ettad.Data.Entities;
using Ettad.Data.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.EntityFramework.Configurations
{
    public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.ToTable("Settings");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Columns
            builder.Property(x => x.Key)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired(false); // nullable

            builder.Property(x => x.Value)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired(false); // nullable

            builder.Property(x => x.Group)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired(false); // nullable
            builder.HasData(
       new Settings
       {
           Id = 1,
           Key = "LdapServer",
           Value = "LDAP://10.80.74.5",
           Group = "LDAP"
       },
       new Settings
       {
           Id = 2,
           Key = "LdapDomain",
           Value = "example.com",
           Group = "LDAP"
       },
       new Settings
       {
           Id = 3,
           Key = "LdapEmpAttr",
           Value = "sAMAccountName",
           Group = "LDAP"
       },
       new Settings
       {
           Id = 4,
           Key = "LdapUsername",
           Value = "ldapuser",
           Group = "LDAP"
       },
       new Settings
       {
           Id = 5,
           Key = "LdapPassword",
           Value = "pass@123",
           Group = "LDAP"
       }
   );
        }
    } 
}
