
namespace Ettad.Api.Hosting;

public static class WebApplicationBuilderExtensions
{
    public const string FrontendCorsPolicy = "FrontendCors";

    public static IServiceCollection AddEttadControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(Inventory.API.Controllers.AmmunitionController).Assembly)
            .AddApplicationPart(typeof(Workflows.API.Controllers.WorkflowsController).Assembly)
            .AddApplicationPart(typeof(User.API.Controllers.UsersController).Assembly)
            .AddApplicationPart(typeof(Lookups.Domain.API.Controllers.DepartmentController).Assembly)
            .AddApplicationPart(typeof(RequestManagement.API.Controllers.OrderController).Assembly)
            .AddApplicationPart(typeof(Notification.API.Controllers.NotificationController).Assembly)
            .AddApplicationPart(typeof(Workflows.API.Controllers.WorkflowApprovalController).Assembly)
            .AddApplicationPart(typeof(Modules.EmailSystem.API.Controllers.EmailSettingsController).Assembly)
            .AddApplicationPart(typeof(Modules.FileUpload.API.Controllers.FileUploadController).Assembly)
            .AddApplicationPart(typeof(LdapSettings.APIs.Controllers.LdapSettingsController).Assembly)
            .AddApplicationPart(typeof(Modules.ReportManagement.API.Controllers.ReportController).Assembly)
            .AddApplicationPart(typeof(Modules.ReportManagement.API.Controllers.CustomQueryBuilderController).Assembly)
            .AddApplicationPart(typeof(Modules.ReportManagement.API.Controllers.CustomReportDesignerController).Assembly)
            .AddApplicationPart(typeof(Modules.ReportManagement.API.Controllers.CustomWebDocumentViewerController).Assembly)
            .AddApplicationPart(typeof(Announcement.API.Controllers.AnnouncementController).Assembly)
            .AddApplicationPart(typeof(HelpCenter.API.Controllers.HelpCenterController).Assembly)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        return services;
    }

    public static IServiceCollection AddEttadReporting(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.ConfigureReportingServices(configurator =>
        {
            if (environment.IsDevelopment())
                configurator.UseDevelopmentMode();

            configurator.ConfigureReportDesigner(designerConfigurator =>
            {
                designerConfigurator.EnableCustomSql();
                designerConfigurator.RegisterDataSourceWizardConfigurationConnectionStringsProvider(
                    configuration.GetSection("ConnectionStrings"));
            });

            configurator.ConfigureWebDocumentViewer(viewerConfigurator =>
            {
                viewerConfigurator.UseCachedReportSourceBuilder();
            });
        });

        return services;
    }

    public static IServiceCollection AddEttadCoreInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddAuthentication(IISDefaults.AuthenticationScheme);
        services.AddMemoryCache();
        services.AddDevExpressControls();
        services.AddEndpointsApiExplorer();

        services.AddInfrastructureServices(configuration);
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<Ettad.CrossCutting.Comman.Time.IDateTimeProvider,Ettad.CrossCutting.Comman.Time.SystemDateTimeProvider>();
        services.AddScoped<IEmailDispatchService, EmailDispatchService>();

        var hangfireConnectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(Hangfire.CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(hangfireConnectionString));
        services.AddHangfireServer();

        services.Configure<JwtOptions>(configuration.GetSection("JWT"));
        services.Configure<FileSettings>(configuration.GetSection("FileSettings"));

        return services;
    }

    public static IServiceCollection AddEttadFeatureModules(this IServiceCollection services)
    {
        services.AddAnnouncementServices();
        services.AddLookUpervices();
        services.AddUserServices();
        services.AddWorkflowServices();
        services.AddHelpCenterServices();
        services.AddInventoryServices();
        services.AddLdapSettingsServices();
        services.AddNotificationServices();
        services.AddReportManagementServices();
        services.AddRequestServices();
        return services;
    }

    public static IServiceCollection AddEttadPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<SoftDeleteInterceptor>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            var interceptor = serviceProvider.GetRequiredService<SoftDeleteInterceptor>();
            options.AddInterceptors(interceptor);
        });

        return services;
    }

    public static IServiceCollection AddEttadIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 5;
            }).AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(15);
        });

        return services;
    }

    public static IServiceCollection AddEttadJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => JwtBearerOptionsConfigurator.Configure(options, configuration));

        return services;
    }

    public static IServiceCollection AddEttadSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName);
            options.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
            options.MapType<FileStream>(() => new OpenApiSchema { Type = "string", Format = "binary" });
            options.OperationFilter<FileUploadOperationFilter>();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

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

        return services;
    }

    public static IServiceCollection AddEttadCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        if (allowedOrigins == null || allowedOrigins.Length == 0)
        {
            allowedOrigins =
            [
                "http://localhost:4200",
                "http://localhost:9090",
                "http://10.80.71.3:9090",
                "http://10.80.71.3"
            ];
        }

        services.AddCors(options =>
        {
            options.AddPolicy(FrontendCorsPolicy, policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
