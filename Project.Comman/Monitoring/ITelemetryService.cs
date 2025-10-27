using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ettad.Shared.Common.Monitoring
{
    public interface ITelemetryService
    {
        void TrackEvent(string eventName, Dictionary<string, string> properties = null);
        void TrackException(Exception exception, Dictionary<string, string> properties = null);
        void TrackDependency(string dependencyType, string target, string name, DateTimeOffset startTime, TimeSpan duration, bool success);
        void TrackTrace(string message, SeverityLevel severity, Dictionary<string, string> properties = null);
    }

    public enum SeverityLevel
    {
        Verbose,
        Information,
        Warning,
        Error,
        Critical
    }
} 