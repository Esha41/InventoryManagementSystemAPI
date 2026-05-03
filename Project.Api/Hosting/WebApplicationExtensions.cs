using Ettad.EntityFramework.DataBaseContext.DataSeeding;
using Ettad.Notification.Service.Hubs;

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

        var scheduleSetting = await context.Settings
            .FirstOrDefaultAsync(s =>
                s.Key == LowStockMonitorConstants.SCHEDULE_SETTINGS_KEY &&
                s.Group == LowStockMonitorConstants.SCHEDULE_SETTINGS_GROUP);

        var lowStockCronExpression = scheduleSetting?.Value
            ?? app.Configuration.GetValue<string>("BackgroundJobs:LowStockMonitor:CronExpression")
            ?? LowStockMonitorConstants.DEFAULT_CRON_EXPRESSION;

        recurringJobManager.AddOrUpdate<LowStockMonitorJob>(
            LowStockMonitorConstants.JOB_ID,
            job => job.ExecuteAsync(),
            lowStockCronExpression);

        Log.Information("Low Stock Monitor job registered with schedule: {Schedule}", lowStockCronExpression);

        var orderAutoRejectCron = app.Configuration.GetValue<string>("BackgroundJobs:OrderAutoReject:CronExpression")
            ?? OrderAutoRejectConstants.DefaultCronExpression;
        var orderAutoRejectEnabled = true;

        try
        {
            var policy = await context.OrderAutoRejectPolicies.AsNoTracking().FirstOrDefaultAsync();
            if (policy != null)
            {
                orderAutoRejectCron = string.IsNullOrWhiteSpace(policy.ScanCron)
                    ? OrderAutoRejectConstants.DefaultCronExpression
                    : policy.ScanCron.Trim();
                orderAutoRejectEnabled = policy.IsEnabled;
            }
            else
            {
                var orderAutoRejectCronSetting = await context.Settings
                    .FirstOrDefaultAsync(s =>
                        s.Key == OrderAutoRejectConstants.ScanCronKey && s.Group == OrderAutoRejectConstants.Group);
                orderAutoRejectCron = orderAutoRejectCronSetting?.Value?.Trim()
                    ?? app.Configuration.GetValue<string>("BackgroundJobs:OrderAutoReject:CronExpression")
                    ?? OrderAutoRejectConstants.DefaultCronExpression;
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex,
                "Could not read OrderAutoRejectPolicies; using Settings/appsettings for order auto-reject cron.");
            var orderAutoRejectCronSetting = await context.Settings
                .FirstOrDefaultAsync(s =>
                    s.Key == OrderAutoRejectConstants.ScanCronKey && s.Group == OrderAutoRejectConstants.Group);
            orderAutoRejectCron = orderAutoRejectCronSetting?.Value?.Trim()
                ?? app.Configuration.GetValue<string>("BackgroundJobs:OrderAutoReject:CronExpression")
                ?? OrderAutoRejectConstants.DefaultCronExpression;
        }

        if (orderAutoRejectEnabled)
        {
            recurringJobManager.AddOrUpdate<OrderAutoRejectHangfireJob>(
                OrderAutoRejectConstants.JobId,
                job => job.ExecuteAsync(),
                orderAutoRejectCron,
                new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
            Log.Information("Order auto-reject job registered with schedule: {Schedule}", orderAutoRejectCron);
        }
        else
        {
            recurringJobManager.RemoveIfExists(OrderAutoRejectConstants.JobId);
            Log.Information("Order auto-reject recurring job removed (disabled in policy).");
        }
    }
}
