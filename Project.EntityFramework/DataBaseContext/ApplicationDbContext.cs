using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Ettad.Data.Entities;
using Ettad.Comman.Enums;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;

using Ettad.Data.Entities.Workflows;
using Ettad.Data.Entities.Settings;

namespace Ettad.EntityFramework.DataBaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
     
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowApprovalHistory> WorkflowApprovalHistory { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }

        public DbSet<EmailConfiguration> EmailConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        
            SeedRoles(modelBuilder);
           
            //SeedTranslations(modelBuilder);
            //SeedDefaultUserAsync();
          // SeedDefaultUserAsync();
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "User",
                NormalizedName = "USER"
            },
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            }
        };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }



        //private void SeedTranslations(ModelBuilder modelBuilder)
        //{
            

        //    modelBuilder.Entity<OrganizationTranslation>().HasData(
        //        new OrganizationTranslation
        //        {
        //            Id = 1, 
        //            OrganizationId = 1, 
        //            LanguageCode = "en",
        //            JsonData = TranslationData.English 
        //        },
        //        new OrganizationTranslation
        //        {
        //            Id = 2, 
        //            OrganizationId = 1,
        //            LanguageCode = "ar",
        //            JsonData = TranslationData.Arabic 
        //        }
        //    );
        //}


        public async Task<bool> TableExistsAsync(string tableName)
        {
            var connection = Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
            var tables = await connection.GetSchemaAsync("Tables");
            return tables.Rows
                .OfType<System.Data.DataRow>()
                .Any(row => row["TABLE_NAME"].ToString().ToLower() == tableName.ToLower());
        }



    }
}