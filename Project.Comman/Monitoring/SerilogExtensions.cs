using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Ettad.CrossCutting.Comman.Monitoring
{
    /// <summary>
    /// Extension methods for Serilog configuration
    /// </summary>
    public static class SerilogExtensions
    {
        /// <summary>
        /// Adds Serilog request logging middleware to the application pipeline
        /// </summary>
        public static IApplicationBuilder UseSerilogRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SerilogRequestLogger>();
        }

        /// <summary>
        /// Adds custom enrichers for Serilog
        /// </summary>
        public static LoggerConfiguration WithUserEnricher(this LoggerConfiguration loggerConfiguration, IServiceProvider serviceProvider)
        {
            var httpContextAccessor = serviceProvider.GetService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            if (httpContextAccessor != null)
            {
                return loggerConfiguration.Enrich.With(new UserEnricher(httpContextAccessor));
            }
            return loggerConfiguration;
        }
    }
}

