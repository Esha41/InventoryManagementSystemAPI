using System;
using Microsoft.Extensions.DependencyInjection;
using Ettad.Shared.Common.Caching;
using Ettad.Shared.Common.Messaging;
using Ettad.Shared.Common.BackgroundJobs;
using Ettad.Shared.Common.Monitoring;
using Hangfire;

namespace Ettad.Shared.Common.Configuration
{
    public static class CrossCuttingConfiguration
    {
        public static IServiceCollection AddCrossCuttingConcerns(
            this IServiceCollection services,
            string redisConnectionString,
            string rabbitMqConnectionString,
            string hangfireConnectionString,
            string applicationInsightsKey)
        {
            // Cache Service
            services.AddSingleton<ICacheService>(provider => 
                new RedisCacheService(redisConnectionString));

            // Message Bus
            services.AddSingleton<IMessageBus>(provider => 
                new RabbitMQMessageBus(rabbitMqConnectionString));

            // Background Jobs
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(hangfireConnectionString));

            services.AddHangfireServer();
            services.AddSingleton<IBackgroundJobService, HangfireJobService>();

            // Monitoring
            //var monitoringService = new ApplicationInsightsService(applicationInsightsKey);
            //services.AddSingleton<IMetricsService>(monitoringService);
            //services.AddSingleton<ITelemetryService>(monitoringService);

            return services;
        }
    }
} 