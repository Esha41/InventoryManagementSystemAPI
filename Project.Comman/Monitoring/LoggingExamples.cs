using Microsoft.Extensions.Logging;
using System;

namespace Ettad.CrossCutting.Comman.Monitoring
{
    /// <summary>
    /// Example class demonstrating Serilog usage patterns
    /// </summary>
    public static class LoggingExamples
    {
        /// <summary>
        /// Example: Information logging
        /// </summary>
        public static void LogInformationExample(ILogger logger, string userName, string action)
        {
            logger.LogInformation("User {UserName} performed {Action}", userName, action);
        }

        /// <summary>
        /// Example: Warning logging
        /// </summary>
        public static void LogWarningExample(ILogger logger, string message, int attemptCount)
        {
            logger.LogWarning("Warning: {Message}. Attempt {AttemptCount}", message, attemptCount);
        }

        /// <summary>
        /// Example: Error logging with exception
        /// </summary>
        public static void LogErrorExample(ILogger logger, System.Exception exception, string context)
        {
            logger.LogError(exception, "Error occurred in {Context}", context);
        }

        /// <summary>
        /// Example: Structured logging with multiple properties
        /// </summary>
        public static void LogStructuredExample(ILogger logger, string employeeCode, int organizationId, string operation)
        {
            logger.LogInformation(
                "Employee {EmployeeCode} in Organization {OrganizationId} performed {Operation}",
                employeeCode,
                organizationId,
                operation
            );
        }

        /// <summary>
        /// Example: Performance tracking
        /// </summary>
        public static void LogPerformanceExample(ILogger logger, string operation, long durationMs)
        {
            if (durationMs > 1000)
            {
                logger.LogWarning(
                    "Slow operation detected: {Operation} took {DurationMs}ms",
                    operation,
                    durationMs
                );
            }
            else
            {
                logger.LogInformation(
                    "Operation {Operation} completed in {DurationMs}ms",
                    operation,
                    durationMs
                );
            }
        }

        /// <summary>
        /// Example: Logging with complex objects (will be serialized as JSON)
        /// </summary>
        public static void LogComplexObjectExample<T>(ILogger logger, T data, string operation)
        {
            logger.LogInformation(
                "Operation {Operation} completed with data: {@Data}",
                operation,
                data // @ symbol serializes the object
            );
        }
    }
}

