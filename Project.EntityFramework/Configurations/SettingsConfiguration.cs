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
           Value = "10.80.70.3",
           Group = "LDAP"
       },
       new Settings
       {
           Id = 2,
           Key = "LdapDomain",
           Value = "sddev.local",
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
           Value = "1000",
           Group = "LDAP"
       },
       new Settings
       {
           Id = 5,
           Key = "LdapPassword",
           Value = "Qatar@2025",
           Group = "LDAP"
       }
   );
        }
    } 
}
