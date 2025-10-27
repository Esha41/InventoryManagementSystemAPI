using System;
using System.Collections.Generic;

namespace Ettad.Shared.Common.Monitoring
{
    public interface IMetricsService
    {
        void RecordMetric(string name, double value, MetricType type, Dictionary<string, string> tags = null);
        void IncrementCounter(string name, Dictionary<string, string> tags = null);
        IDisposable MeasureOperation(string name, Dictionary<string, string> tags = null);
    }

    public enum MetricType
    {
        Counter,
        Gauge,
        Histogram,
        Timer
    }
} 