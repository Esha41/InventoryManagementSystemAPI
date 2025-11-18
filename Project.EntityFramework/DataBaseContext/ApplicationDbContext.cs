using System;
using Ettad.Comman.Enums;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Settings;
using Ettad.Data.Entities.Workflows;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Ettad.EntityFramework.DataBaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStepApprovalLog> WorkflowStepApprovalLog { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }
        public DbSet<WorkflowApprovalStep> WorkflowApprovalSteps { get; set; }
        public DbSet<EmailConfiguration> EmailConfigurations { get; set; }

        public DbSet<BaseItem> BaseItems { get; set; }
        public DbSet<Ammunition> Ammunitions { get; set; }
        public DbSet<AllowanceItem> AllowanceItems { get; set; }
        public DbSet<CaseType> CaseTypes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Compatibility> Compatibilities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Depot> Depots { get; set; }
        public DbSet<HazardDivision> HazardDivisions { get; set; }
        public DbSet<Hcc> Hcc { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryDetail> InventoryDetails { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<NatureOption> NatureOptions { get; set; }
        public DbSet<PrimaryPurpos> PrimaryPurposes { get; set; }
        public DbSet<ProjectailMaterial> ProjectailMaterials { get; set; }
        public DbSet<Propellant> Propellants { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Rank> Ranks { get; set; }

        public DbSet<RequestPurpose> RequestPurposes { get; set; }
        public DbSet<BaseRequest> BaseRequests { get; set; }
        public DbSet<RequestItem> RequestItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Discard> Discards { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<SupplyDetail> SupplyDetails { get; set; }
     

        public DbSet<ApplicationEntity> ApplicationEntities { get;set;}
        public DbSet<RoleApplicationEntity> RoleApplicationEntities { get; set; }
        public DbSet<WorkFlowType> WorkFlowTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationReceiver> NotificationReceivers { get; set; }
        public DbSet<Settings> Settings { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            //SeedRoles(modelBuilder);
            //SeedTranslations(modelBuilder);
            //SeedDefaultUserAsync();
            // SeedDefaultUserAsync();
        }

        //private void SeedRoles(ModelBuilder modelBuilder)
        //{
        //    var roles = new List<IdentityRole>
        //    {
        //        new IdentityRole
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            Name = "User",
        //            NormalizedName = "USER"
        //        },
        //        new IdentityRole
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            Name = "Admin",
        //            NormalizedName = "ADMIN"
        //        }
        //    };

        //    modelBuilder.Entity<IdentityRole>().HasData(roles);
        //}



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