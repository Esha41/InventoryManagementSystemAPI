
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Monitoring.BackgroundJobs;
using Ettad.ReportManagement.Service.BackgroundJob;

namespace Ettad.Api.Hosting;

public static class WebApplicationExtensions
{
    public static void UseEttadSerilogRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(configure =>
        {
            configure.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
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
                    diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
            };
        });
    }

    public static void UseEttadMiddlewarePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ettad API V1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseStaticFiles();
        app.UseDevExpressControls();
        app.UseHttpsRedirection();

        if (!app.Environment.IsDevelopment())
            app.UseHsts();

        app.UseCors(WebApplicationBuilderExtensions.FrontendCorsPolicy);
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
        app.MapHub<NotificationHub>("/hubs/notification");
    }

    public static async Task RunEttadDatabaseInitializationAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        try
        {
            await ApplicationDbInitializer.ApplyPendingMigrationsAsync(scope.ServiceProvider);

            var context = services.GetRequiredService<ApplicationDbContext>();
            var environment = services.GetService<IWebHostEnvironment>();
            await ApplicationDbcontextSeed.SeedDefaultUserAsync(context, userManager, roleManager, environment);
            await ApplicationDbInitializer.SeedDefaultDataAsync(scope.ServiceProvider);

            Log.Information("Database migration and seeding completed successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred during database migration or seeding");
            throw;
        }
    }

    public static async Task RegisterEttadHangfireRecurringJobsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await RegisterLowStockMonitorRecurringJobAsync(app, recurringJobManager, context).ConfigureAwait(false);
        await RegisterCriticalStockMonitorRecurringJobAsync(app, recurringJobManager, context).ConfigureAwait(false);
        await RegisterOrderAutoRejectRecurringJobAsync(app, recurringJobManager, context).ConfigureAwait(false);
        await RegisterScheduledReportsRecurringJob(app, recurringJobManager, context);
    }

    private static async Task RegisterLowStockMonitorRecurringJobAsync(
        WebApplication app,
        IRecurringJobManager recurringJobManager,
        ApplicationDbContext context)
    {
        var scheduleSetting = await context.Settings
            .FirstOrDefaultAsync(s =>
                s.Key == LowStockMonitorConstants.SCHEDULE_SETTINGS_KEY &&
                s.Group == LowStockMonitorConstants.SCHEDULE_SETTINGS_GROUP)
            .ConfigureAwait(false);

        var cronExpression = scheduleSetting?.Value
            ?? app.Configuration.GetValue<string>("BackgroundJobs:LowStockMonitor:CronExpression")
            ?? LowStockMonitorConstants.DEFAULT_CRON_EXPRESSION;

        recurringJobManager.AddOrUpdate<LowStockMonitorJob>(
            LowStockMonitorConstants.JOB_ID,
            job => job.ExecuteAsync(),
            cronExpression,
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });

        Log.Information(
            "Low Stock Monitor job registered: cron={Cron} hangfireTz={Tz}",
            cronExpression,
            TimeZoneInfo.Local.Id);
    }

    private static async Task RegisterCriticalStockMonitorRecurringJobAsync(
        WebApplication app,
        IRecurringJobManager recurringJobManager,
        ApplicationDbContext context)
    {
        var scheduleSetting = await context.Settings
            .FirstOrDefaultAsync(s =>
                s.Key == CriticalStockMonitorConstants.SCHEDULE_SETTINGS_KEY &&
                s.Group == CriticalStockMonitorConstants.SCHEDULE_SETTINGS_GROUP)
            .ConfigureAwait(false);

        var cronExpression = scheduleSetting?.Value
            ?? app.Configuration.GetValue<string>("BackgroundJobs:CriticalStockMonitor:CronExpression")
            ?? CriticalStockMonitorConstants.DEFAULT_CRON_EXPRESSION;

        recurringJobManager.AddOrUpdate<CriticalStockMonitorJob>(
            CriticalStockMonitorConstants.JOB_ID,
            job => job.ExecuteAsync(),
            cronExpression,
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });

        Log.Information(
            "Critical Stock Monitor job registered: cron={Cron} hangfireTz={Tz}",
            cronExpression,
            TimeZoneInfo.Local.Id);
    }

    private static async Task RegisterOrderAutoRejectRecurringJobAsync(
        WebApplication app,
        IRecurringJobManager recurringJobManager,
        ApplicationDbContext context)
    {
        var (cronExpression, enabled) = await ResolveOrderAutoRejectScheduleAsync(app, context).ConfigureAwait(false);

        if (enabled)
        {
            recurringJobManager.AddOrUpdate<OrderAutoRejectHangfireJob>(
                OrderAutoRejectConstants.JobId,
                job => job.ExecuteAsync(),
                cronExpression,
                new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
            Log.Information("Order auto-reject job registered with schedule: {Schedule}", cronExpression);
        }
        else
        {
            recurringJobManager.RemoveIfExists(OrderAutoRejectConstants.JobId);
            Log.Information("Order auto-reject recurring job removed (disabled in policy).");
        }
    }

    private static async Task<(string CronExpression, bool Enabled)> ResolveOrderAutoRejectScheduleAsync(
        WebApplication app,
        ApplicationDbContext context)
    {
        var cron = app.Configuration.GetValue<string>("BackgroundJobs:OrderAutoReject:CronExpression")
            ?? OrderAutoRejectConstants.DefaultCronExpression;
        var enabled = true;

        try
        {
            var policy = await context.OrderAutoRejectPolicies.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false);
            if (policy != null)
            {
                cron = string.IsNullOrWhiteSpace(policy.ScanCron)
                    ? OrderAutoRejectConstants.DefaultCronExpression
                    : policy.ScanCron.Trim();
                enabled = policy.IsEnabled;
            }
            else
            {
                var setting = await context.Settings
                    .FirstOrDefaultAsync(s =>
                        s.Key == OrderAutoRejectConstants.ScanCronKey && s.Group == OrderAutoRejectConstants.Group)
                    .ConfigureAwait(false);
                cron = setting?.Value?.Trim()
                    ?? app.Configuration.GetValue<string>("BackgroundJobs:OrderAutoReject:CronExpression")
                    ?? OrderAutoRejectConstants.DefaultCronExpression;
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex,
                "Could not read OrderAutoRejectPolicies; using Settings/appsettings for order auto-reject cron.");
            var setting = await context.Settings
                .FirstOrDefaultAsync(s =>
                    s.Key == OrderAutoRejectConstants.ScanCronKey && s.Group == OrderAutoRejectConstants.Group)
                .ConfigureAwait(false);
            cron = setting?.Value?.Trim()
                ?? app.Configuration.GetValue<string>("BackgroundJobs:OrderAutoReject:CronExpression")
                ?? OrderAutoRejectConstants.DefaultCronExpression;
        }

        return (cron, enabled);
    }

    private static async Task RegisterScheduledReportsRecurringJob(WebApplication app, IRecurringJobManager recurringJobManager, ApplicationDbContext context)
    {
        var schedules = await context.ScheduledReports.Where(x => x.IsActive && !x.IsDeleted).ToListAsync();

        foreach (var schedule in schedules)
        {
            var cron = BuildCron(schedule);

            recurringJobManager.AddOrUpdate<ScheduledReportJob>(
                $"scheduled-report-{schedule.Id}",
                job => job.ExecuteAsync(),
                cron,
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Local
                });
        }

        Log.Information(
            "Scheduled reports Hangfire poll registered: jobId={JobId}, cron={Cron}, hangfireTz={Tz}",
            TimeZoneInfo.Local.Id);
    }

    private static string BuildCron(ScheduledReport schedule)
    {
        var parts = schedule.TimeOfDay.Split(':');

        var hour = parts[0];
        var minute = parts[1];

        if (schedule.Frequency == "Weekly")
            return $"{minute} {hour} * * {schedule.DayOfWeek}";

        if (schedule.Frequency == "Monthly")
            return $"{minute} {hour} {schedule.DayOfMonth.Value} * *";

        // Daily
        return $"{minute} {hour} * * *";
    }
}
