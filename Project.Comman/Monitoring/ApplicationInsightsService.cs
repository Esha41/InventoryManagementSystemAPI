using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Ettad.Shared.Common.Monitoring
{
    public class ApplicationInsightsService : IMetricsService, ITelemetryService
    {
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsService(string instrumentationKey)
        {
            var config = new TelemetryConfiguration(instrumentationKey);
            _telemetryClient = new TelemetryClient(config);
        }

        // IMetricsService implementation
        public void RecordMetric(string name, double value, MetricType type, Dictionary<string, string> tags = null)
        {
            switch (type)
            {
                case MetricType.Counter:
                    _telemetryClient.TrackMetric(name, value, tags);
                    break;
                case MetricType.Gauge:
                    _telemetryClient.GetMetric(name).TrackValue(value);
                    break;
                case MetricType.Histogram:
                    _telemetryClient.TrackMetric(name, value, tags);
                    break;
                case MetricType.Timer:
                    _telemetryClient.TrackMetric(name, value, tags);
                    break;
            }
        }

        public void IncrementCounter(string name, Dictionary<string, string> tags = null)
        {
            _telemetryClient.TrackMetric(name, 1, tags);
        }

        public IDisposable MeasureOperation(string name, Dictionary<string, string> tags = null)
        {
            var operation = _telemetryClient.StartOperation<DependencyTelemetry>(name);
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    operation.Telemetry.Properties[tag.Key] = tag.Value;
                }
            }
            return operation;
        }

        // ITelemetryService implementation
        public void TrackEvent(string eventName, Dictionary<string, string> properties = null)
        {
            _telemetryClient.TrackEvent(eventName, properties);
        }

        public void TrackException(Exception exception, Dictionary<string, string> properties = null)
        {
            _telemetryClient.TrackException(exception, properties);
        }

        public void TrackDependency(string dependencyType, string target, string name, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            var telemetry = new DependencyTelemetry(dependencyType, target, name, null, startTime, duration, null, success);
            _telemetryClient.TrackDependency(telemetry);
        }

        public void TrackTrace(string message, SeverityLevel severity, Dictionary<string, string> properties = null)
        {
            var severityLevel = severity switch
            {
                SeverityLevel.Verbose => Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Verbose,
                SeverityLevel.Information => Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information,
                SeverityLevel.Warning => Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning,
                SeverityLevel.Error => Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error,
                SeverityLevel.Critical => Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Critical,
                _ => Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information
            };

            _telemetryClient.TrackTrace(message, severityLevel, properties);
        }
    }
}