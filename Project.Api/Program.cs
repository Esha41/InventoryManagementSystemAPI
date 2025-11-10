using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Monitoring;
//using Ettad.Module.Logic.Extensions;
//using Mujam.Intergration.Service.Mangment;
using Ettad.CrossCutting.Data.Repository;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.EntityFramework.DataBaseContext.DataSeeding;
using Ettad.Lookups.Services.Contracts;
using Ettad.Lookups.Services.Implementation;
using Ettad.Repository;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Helpers;
using Ettad.User.Services.Interfaces;
using Ettad.Workflow.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Moujam.Casiher.Comman.Models;
using Serilog;
using Serilog.Events;
using Ettad.Inventory.Service;
using System.Text;
using System.Text.Json;
using Ettad.EntityFramework.Interceptors;
using Ettad.RequestManagement.Service;
using System.Reflection;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting Ettad Backend API...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Ensure Log Database exists before Serilog starts
    DatabaseHelper.EnsureLogDatabaseExists(builder.Configuration);

    // Add Serilog to the application
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WithUserEnricher(services));

    var configuration = builder.Configuration;

    // Add HttpContextAccessor for Serilog enrichers and CurrentUserService
    builder.Services.AddHttpContextAccessor();

    // Register ICurrentUserService early so interceptor can use it
    builder.Services.AddScoped<Ettad.Application.Common.Interfaces.ICurrentUserService, Ettad.User.Services.Implementation.CurrentUserService>();

    // Add services to the container.
    // Register controllers from all referenced assemblies
    builder.Services.AddControllers()
        .AddApplicationPart(typeof(Ettad.Inventory.API.Controllers.AmmunitionController).Assembly)
        .AddApplicationPart(typeof(Ettad.Workflows.API.Controllers.WorkflowsController).Assembly)
        .AddApplicationPart(typeof(Ettad.User.API.Controllers.UsersController).Assembly)
        .AddApplicationPart(typeof(Ettad.Lookups.Domain.API.Controllers.DepartmentController).Assembly)
        .AddApplicationPart(typeof(Ettad.RequestManagement.API.Controllers.OrderController).Assembly)
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddScoped(typeof(CrossCuttingRepository<>));

    builder.Services.AddScoped(typeof(ICrossCuttingRepository<>), typeof(CrossCuttingRepository<>));

    builder.Services.AddScoped(typeof(ILookupService<,>), typeof(LookupService<,>));

    builder.Services.AddScoped<IEmailSender, EmailSender>();

    builder.Services.AddScoped<IFileStorageService, FileStorageService>();

    builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("JWT"));

    #region Register Modules
    builder.Services.AddInventoryServices();
    builder.Services.AddRequestServices();
    #endregion

    // Register soft delete interceptor (ICurrentUserService is already registered above)
    builder.Services.AddScoped<SoftDeleteInterceptor>();

    #region Connection String
    builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

        // Get the interceptor from service provider
        var interceptor = serviceProvider.GetRequiredService<SoftDeleteInterceptor>();
        options.AddInterceptors(interceptor);
    });
    #endregion

    #region Identity
    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequiredLength = 5;
    }).AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
    #endregion
    builder.Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
               .AddJwtBearer(options =>
               {
                   options.SaveToken = true;
                   options.RequireHttpsMetadata = false;

                   var validIssuers = configuration.GetSection("JWT:ValidIssuers").Get<string[]>();
                   var validAudiences = configuration.GetSection("JWT:ValidAudiences").Get<string[]>();

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = false,
                       //ValidIssuer = configuration["JWT:ValidIssur"],
                       //   ValidIssuers = validIssuers,
                       ValidateAudience = false,
                       //ValidAudiences = validAudiences,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT Secret is missing")))
                   };
               });
    var baseUrl = builder.Configuration.GetValue<string>("FileSettings:BASE_URL");
    if (!string.IsNullOrEmpty(baseUrl))
    {
        baseUrl = baseUrl.Remove(baseUrl.LastIndexOf("/"));
    }
    builder.Services.Configure<FileSettings>(builder.Configuration.GetSection("FileSettings"));
    //builder.Services.AddRefitClient<IServieMangamentApI>().ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));
    #region Dependency Injection
    builder.Services.AddInfrastructureServices();

    Ettad.User.Services.ModuleServicesDependences.AddReposetoriesServices(builder.Services);

    // Register soft delete interceptor after ICurrentUserService is registered
    builder.Services.AddScoped<SoftDeleteInterceptor>();

    builder.Services.AddAutoMapper(typeof(Ettad.Module.lookup.Mapper.LookupMappingProfile));

    // Register Employee services directly
    builder.Services.AddScoped<IUserService, UserService>();

    //builder.Services.AddAutoMapper(typeof(Ettad.Module.lookup.Mapper.LookupMappingProfile));

    // Register HR services

    builder.Services.AddWorkflowServices();
    // Register Attendance services

    // Configure Swagger to include all controllers from referenced assemblies
    builder.Services.AddSwaggerGen(options =>
    {
        options.CustomSchemaIds(type => type.FullName);

        // Add security definition for Bearer token
        options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        // Add security requirement
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
        });
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            policy => policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });


    #endregion

    var app = builder.Build();

    // Configure Serilog request logging
    app.UseSerilogRequestLogging(configure =>
    {
        configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        configure.GetLevel = (httpContext, elapsed, ex) => ex != null
            ? LogEventLevel.Error
            : httpContext.Response.StatusCode > 499
                ? LogEventLevel.Error
                : LogEventLevel.Information;
        configure.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString());

            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
            }
        };
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseStaticFiles();
    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();
    app.MapControllers();
    app.UseCors("AllowAll");

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
            }
            await ApplicationDbcontextSeed.SeedDefaultUserAsync(context, userManager, roleManager);
            await Ettad.EntityFramework.DataBaseContext.DataSeeding.ApplicationDbInitializer.SeedDefaultDataAsync(scope.ServiceProvider);

            Log.Information("Database migration and seeding completed successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred during database migration or seeding");
            //  await ApplicationDbInitializer.SeedDefaultDataAsync(scope.ServiceProvider);
        }
    }

    Log.Information("Ettad Backend API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.Information("Shutting down Ettad Backend API");
    Log.CloseAndFlush();
}

/// <summary>
/// Helper methods for database setup
/// </summary>
static class DatabaseHelper
{
    /// <summary>
    /// Ensures the Log Database exists before the application starts logging
    /// Similar to how EttadDb is created via context.Database.Migrate()
    /// </summary>
    public static void EnsureLogDatabaseExists(IConfiguration configuration)
    {
        try
        {
            var logConnectionString = configuration.GetConnectionString("LogConnection");
            if (string.IsNullOrEmpty(logConnectionString))
            {
                Log.Warning("LogConnection string not found. Skipping log database creation.");
                return;
            }

            // Parse connection string to get database name
            var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(logConnectionString);
            var databaseName = builder.InitialCatalog;
            var masterConnectionString = logConnectionString.Replace(databaseName, "master");

            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(masterConnectionString))
            {
                connection.Open();
                
                // Check if database exists
                var checkDbCommand = connection.CreateCommand();
                checkDbCommand.CommandText = $"SELECT database_id FROM sys.databases WHERE Name = '{databaseName}'";
                var exists = checkDbCommand.ExecuteScalar();

                if (exists == null)
                {
                    // Create database
                    var createDbCommand = connection.CreateCommand();
                    createDbCommand.CommandText = $"CREATE DATABASE [{databaseName}]";
                    createDbCommand.ExecuteNonQuery();
                    
                    Log.Information("Log database '{DatabaseName}' created automatically", databaseName);
                }
                else
                {
                    Log.Information("Log database '{DatabaseName}' already exists", databaseName);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to create log database automatically. It may need to be created manually.");
        }
    }
}
