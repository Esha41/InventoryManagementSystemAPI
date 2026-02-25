using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Monitoring;
using Ettad.CrossCutting.Data.Repository;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.EntityFramework.Interceptors;
using Ettad.Inventory.Service;
using Ettad.Inventory.Service.Monitoring;
using Ettad.LdapSettings.Services;
using Ettad.Lookups.Services.Contracts;
using Ettad.Lookups.Services.Implementation; 
using Ettad.Notification.Service;
using Ettad.Announcement.Service;
using Ettad.Repository;
using Ettad.RequestManagement.Service;
using Ettad.Services;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Helpers;
using Ettad.User.Services.Interfaces;
using Ettad.Workflow.Service;
using Ettad.Workflows.Service.Imeplemention;
using Ettad.Workflows.Service.Interface;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Moujam.Casiher.Comman.Models;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json;

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
    builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);
    // Add MemoryCache for CAPTCHA service
    builder.Services.AddMemoryCache();

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
        .AddApplicationPart(typeof(Ettad.Notification.API.Controllers.NotificationController).Assembly)
        .AddApplicationPart(typeof(Ettad.Workflows.API.Controllers.WorkflowApprovalController).Assembly)
        .AddApplicationPart(typeof(Ettad.Modules.EmailSystem.API.Controllers.EmailSettingsController).Assembly)
        .AddApplicationPart(typeof(Ettad.Modules.FileUpload.API.Controllers.FileUploadController).Assembly)
        .AddApplicationPart(typeof(Ettad.LdapSettings.APIs.Controllers.LdapSettingsController).Assembly)
        .AddApplicationPart(typeof(Ettad.Announcement.API.Controllers.AnnouncementController).Assembly)
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddScoped(typeof(CrossCuttingRepository<>));

    builder.Services.AddScoped(typeof(ICrossCuttingRepository<>), typeof(CrossCuttingRepository<>));

    builder.Services.AddScoped(typeof(ILookupService<,>), typeof(LookupService<,>));

    // Register custom Depot service with inventory validation
    builder.Services.AddScoped<IDepotService, DepotService>();
    builder.Services.AddScoped<Ettad.Lookups.Services.Contracts.IDepotAccessService, Ettad.Lookups.Services.Implementation.DepotAccessService>();
    builder.Services.AddScoped<Ettad.Lookups.Services.Contracts.IUserDepotService, Ettad.Lookups.Services.Implementation.UserDepotService>();

    builder.Services.AddScoped<IEmailSender, EmailSender>();
    builder.Services.AddScoped<IWorkflowApprovalService, WorkflowApprovalService>();
    builder.Services.AddScoped<IFileStorageService, FileStorageService>();
    builder.Services.AddScoped<IFileUploadService, Ettad.Modules.FileUpload.API.Services.FileUploadService>();
    builder.Services.AddScoped<IExcelExportService, ExcelExportService>();
    // Configure Hangfire for background jobs
    var hangfireConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddHangfire(config => config
        .SetDataCompatibilityLevel(Hangfire.CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(hangfireConnectionString));
    builder.Services.AddHangfireServer();

    builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("JWT"));

    #region Register Modules
    builder.Services.AddInventoryServices();
    builder.Services.AddRequestServices();
    builder.Services.AddNotificationServices();
    builder.Services.AddLdapSettingsServices();
    builder.Services.AddAnnouncementServices();
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

    // Configure password reset token to expire in 15 minutes
    builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    {
        options.TokenLifespan = TimeSpan.FromMinutes(15);
    });
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
                   
                   // Add event handler to check if token is blacklisted
                   options.Events = new JwtBearerEvents
                   {
                       OnTokenValidated = async context =>
                       {
                           // Get the token blacklist service from DI
                           var tokenBlacklistService = context.HttpContext.RequestServices
                               .GetRequiredService<Ettad.User.Services.Interfaces.ITokenBlacklistService>();
                           
                           // Extract jti claim from the token
                           var jtiClaim = context.Principal?.Claims
                               .FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti);
                           
                           if (jtiClaim != null && !string.IsNullOrWhiteSpace(jtiClaim.Value))
                           {
                               // Check if token is blacklisted
                               var isBlacklisted = await tokenBlacklistService.IsTokenBlacklistedAsync(
                                   jtiClaim.Value, 
                                   context.HttpContext.RequestAborted);
                               
                               if (isBlacklisted)
                               {
                                   // Token is blacklisted - reject the request
                                   context.Fail("This token has been revoked.");
                                   Log.Warning("Blacklisted token rejected. TokenId: {TokenId}", jtiClaim.Value);
                                   return;
                               }

                               // Single-session: validate CurrentTokenId (reject if session invalidated by new login)
                               // "sub" may be mapped to ClaimTypes.NameIdentifier by default, so check both
                               var userIdClaim = context.Principal?.Claims
                                   .FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub
                                       || c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                               var userId = userIdClaim?.Value;
                               if (!string.IsNullOrWhiteSpace(userId))
                               {
                                   var userManager = context.HttpContext.RequestServices
                                       .GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Ettad.Comman.Idenitity.ApplicationUser>>();
                                   var user = await userManager.FindByIdAsync(userId);
                                   if (user == null || user.CurrentTokenId != jtiClaim.Value)
                                   {
                                       context.Fail("Session invalidated by new login.");
                                       Log.Warning("Token rejected - session invalidated. UserId: {UserId}, TokenId: {TokenId}", userId, jtiClaim.Value);
                                   }
                               }
                           }
                       }
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
        
        // Map enums as strings in Swagger
        options.SchemaFilter<EnumAsStringSchemaFilter>();
        options.ParameterFilter<EnumAsStringParameterFilter>();
        
        // Handle file uploads - map IFormFile to prevent parameter generation errors
        options.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
        options.MapType<FileStream>(() => new OpenApiSchema { Type = "string", Format = "binary" });
        
        // Handle file uploads in Swagger
        options.OperationFilter<FileUploadOperationFilter>();

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

    var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
    if (allowedOrigins == null || allowedOrigins.Length == 0)
    {
        allowedOrigins = new[] {
        "http://localhost:4200",      // Angular dev server (ng serve)
        "http://localhost:9090",      // IIS on localhost
        "http://10.80.71.3:9090",     // IIS on server IP
        "http://10.80.71.3"           // IIS default port
    };
    }

    const string corsPolicyName = "FrontendCors";

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(corsPolicyName, policy =>
        {
            policy
                .WithOrigins(allowedOrigins) // must include frontend URL
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); // keep this if you use cookies/auth
        });
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
    // Add error handling for Swagger - only in Development environment
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/swagger.json";
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ettad API V1");
            c.RoutePrefix = "swagger";
        });
    }
  
    app.UseStaticFiles();
    app.UseHttpsRedirection();

    // Add HSTS (HTTP Strict Transport Security) - only in non-development environments
    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseCors(corsPolicyName);
    app.UseWhen(context => context.Request.Method == "OPTIONS", appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 204;
            await Task.CompletedTask;
        });
    });
    app.Use(async (context, next) =>
    {
        context.Response.Headers["X-Frame-Options"] = "DENY";
        context.Response.Headers["Content-Security-Policy"] = "frame-ancestors 'self'";
        await next();
    });

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.MapHub<Ettad.Notification.Service.Hubs.NotificationHub>("/hubs/notification");
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        try
        {
            // Check and apply pending migrations
            await Ettad.EntityFramework.DataBaseContext.DataSeeding.ApplicationDbInitializer.ApplyPendingMigrationsAsync(scope.ServiceProvider);
            
            var context = services.GetRequiredService<ApplicationDbContext>();
            var environment = services.GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
            await ApplicationDbcontextSeed.SeedDefaultUserAsync(context, userManager, roleManager, environment);
            await Ettad.EntityFramework.DataBaseContext.DataSeeding.ApplicationDbInitializer.SeedDefaultDataAsync(scope.ServiceProvider);

            Log.Information("Database migration and seeding completed successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred during database migration or seeding");
            //  await ApplicationDbInitializer.SeedDefaultDataAsync(scope.ServiceProvider);
        }
    }

    // Register Recurring Jobs
    // Low Stock Monitor Job - Checks items daily and sends notifications when stock is low
    // Schedule is stored in Settings table (Key: "LowStockMonitorSchedule", Group: "BackgroundJobs")
    // Can be updated at runtime via API endpoint
    using (var scope = app.Services.CreateScope())
    {
        var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Read schedule from Settings table, fallback to appsettings.json, then default
        // Note: Cron expressions use local time.
        // Example: 9:15 AM local time = "15 9 * * *"
        var scheduleSetting = await context.Settings
            .FirstOrDefaultAsync(s => s.Key == LowStockMonitorConstants.SCHEDULE_SETTINGS_KEY && s.Group == LowStockMonitorConstants.SCHEDULE_SETTINGS_GROUP);
        
        var lowStockCronExpression = scheduleSetting?.Value 
            ?? app.Configuration.GetValue<string>("BackgroundJobs:LowStockMonitor:CronExpression") 
            ?? LowStockMonitorConstants.DEFAULT_CRON_EXPRESSION; // Default: 9:15 AM local time


        // Register recurring job - Hangfire resolves LowStockMonitorJob from DI at execution time
        recurringJobManager.AddOrUpdate<LowStockMonitorJob>(
            LowStockMonitorConstants.JOB_ID,
            job => job.ExecuteAsync(),
            lowStockCronExpression);
        
        Log.Information("Low Stock Monitor job registered with schedule: {Schedule}", lowStockCronExpression);
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

// Schema filter for enum handling in Swagger
public class EnumAsStringSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        try
        {
            if (context?.Type != null && context.Type.IsEnum && schema != null)
            {
                var enumValues = Enum.GetValues(context.Type);
                if (enumValues != null && enumValues.Length > 0)
                {
                    schema.Type = "string";
                    schema.Format = null;
                    schema.Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>();
                    foreach (var enumValue in enumValues)
                    {
                        if (enumValue != null)
                        {
                            schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString(enumValue.ToString()));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log but don't throw - allow Swagger to continue
            System.Diagnostics.Debug.WriteLine($"EnumAsStringSchemaFilter error: {ex.Message}");
        }
    }
}

// Parameter filter for enum handling in query parameters and file upload exclusion
public class EnumAsStringParameterFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        try
        {
            // Exclude IFormFile parameters from being generated as query/route parameters
            // They should be handled by the operation filter as request body
            if (context?.ParameterInfo?.ParameterType != null)
            {
                var paramType = context.ParameterInfo.ParameterType;
                
                // Skip IFormFile parameters - they'll be handled by operation filter
                if (paramType == typeof(IFormFile) || paramType == typeof(List<IFormFile>))
                {
                    // This will be handled by the operation filter, so we can skip it here
                    return;
                }
                
                // Handle enums
                if (paramType.IsEnum && parameter != null)
                {
                    var enumValues = Enum.GetValues(paramType);
                    
                    if (enumValues != null && enumValues.Length > 0)
                    {
                        var enumList = new List<Microsoft.OpenApi.Any.IOpenApiAny>();
                        foreach (var enumValue in enumValues)
                        {
                            if (enumValue != null)
                            {
                                enumList.Add(new Microsoft.OpenApi.Any.OpenApiString(enumValue.ToString()));
                            }
                        }
                        
                        parameter.Schema = new OpenApiSchema
                        {
                            Type = "string",
                            Enum = enumList
                        };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log but don't throw - allow Swagger to continue
            System.Diagnostics.Debug.WriteLine($"EnumAsStringParameterFilter error: {ex.Message}");
        }
    }
}

// Operation filter to handle file uploads in Swagger
public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Find all IFormFile parameters (with or without [FromForm])
        var fileParameters = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile) || 
                       p.ParameterType == typeof(List<IFormFile>) ||
                       (p.ParameterType.IsGenericType && 
                        p.ParameterType.GetGenericTypeDefinition() == typeof(List<>) &&
                        p.ParameterType.GetGenericArguments()[0] == typeof(IFormFile)))
            .ToList();

        // Also check for other [FromForm] parameters
        var otherFormParameters = context.MethodInfo.GetParameters()
            .Where(p => p.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.FromFormAttribute), false).Any() &&
                       p.ParameterType != typeof(IFormFile) &&
                       p.ParameterType != typeof(List<IFormFile>) &&
                       !(p.ParameterType.IsGenericType && 
                         p.ParameterType.GetGenericTypeDefinition() == typeof(List<>) &&
                         p.ParameterType.GetGenericArguments()[0] == typeof(IFormFile)))
            .ToList();

        if (fileParameters.Any() || otherFormParameters.Any())
        {
            // Remove file and form parameters from the parameters list first
            if (operation.Parameters != null)
            {
                var allFormParams = fileParameters.Concat(otherFormParameters).ToList();
                operation.Parameters = operation.Parameters
                    .Where(p => !allFormParams.Any(fp => fp.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            // Create or update request body for multipart/form-data
            if (operation.RequestBody == null)
            {
                operation.RequestBody = new OpenApiRequestBody();
            }

            if (operation.RequestBody.Content == null)
            {
                operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>();
            }

            if (!operation.RequestBody.Content.ContainsKey("multipart/form-data"))
            {
                operation.RequestBody.Content["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>(),
                        Required = new HashSet<string>()
                    }
                };
            }

            var formDataSchema = operation.RequestBody.Content["multipart/form-data"].Schema;
            if (formDataSchema.Properties == null)
            {
                formDataSchema.Properties = new Dictionary<string, OpenApiSchema>();
            }
            if (formDataSchema.Required == null)
            {
                formDataSchema.Required = new HashSet<string>();
            }

            foreach (var param in fileParameters)
            {
                var isList = param.ParameterType == typeof(List<IFormFile>) ||
                            (param.ParameterType.IsGenericType && 
                             param.ParameterType.GetGenericTypeDefinition() == typeof(List<>));
                
                var schema = isList
                    ? new OpenApiSchema 
                    { 
                        Type = "array", 
                        Items = new OpenApiSchema { Type = "string", Format = "binary" } 
                    }
                    : new OpenApiSchema { Type = "string", Format = "binary" };

                formDataSchema.Properties[param.Name] = schema;
                
                // Mark as required if parameter is not optional
                if (!param.IsOptional)
                {
                    formDataSchema.Required.Add(param.Name);
                }
            }

            // Handle other [FromForm] parameters (non-file parameters)
            foreach (var param in otherFormParameters)
            {
                OpenApiSchema schema;
                
                if (param.ParameterType == typeof(string))
                {
                    schema = new OpenApiSchema { Type = "string" };
                }
                else if (param.ParameterType == typeof(int) || param.ParameterType == typeof(long))
                {
                    schema = new OpenApiSchema { Type = "integer", Format = param.ParameterType == typeof(long) ? "int64" : "int32" };
                }
                else if (param.ParameterType == typeof(bool))
                {
                    schema = new OpenApiSchema { Type = "boolean" };
                }
                else if (param.ParameterType == typeof(DateTime) || param.ParameterType == typeof(DateTime?))
                {
                    schema = new OpenApiSchema { Type = "string", Format = "date-time" };
                }
                else
                {
                    // For complex types, use object schema
                    schema = new OpenApiSchema { Type = "object" };
                }

                formDataSchema.Properties[param.Name] = schema;
                
                if (!param.IsOptional && !IsNullableType(param.ParameterType))
                {
                    formDataSchema.Required.Add(param.Name);
                }
            }
        }
    }

    private static bool IsNullableType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}
