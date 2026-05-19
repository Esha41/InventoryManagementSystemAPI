
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
    var configuration = builder.Configuration;

    // Ensure Log Database exists before Serilog starts
    ApplicationDbInitializer.EnsureLogDatabaseExists(configuration);

    // Add Serilog to the application
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WithUserEnricher(services));

    builder.Services
        .AddEttadCoreInfrastructure(configuration)
        .AddEttadPersistence(configuration)
        .AddEttadIdentity()
        .AddEttadJwtAuthentication(configuration)
        .AddEttadFeatureModules()
        .AddEttadReporting(configuration, builder.Environment)
        .AddEttadControllers()
        .AddEttadSwagger()
        .AddEttadCors(configuration);

    var app = builder.Build();

    await app.RunEttadDatabaseInitializationAsync();
    await app.RegisterEttadHangfireRecurringJobsAsync();

    app.UseEttadSerilogRequestLogging();
    app.UseEttadMiddlewarePipeline();

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
