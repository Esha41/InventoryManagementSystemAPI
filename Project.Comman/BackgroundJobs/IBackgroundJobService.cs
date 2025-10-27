using System;
using System.Threading.Tasks;

namespace Ettad.Shared.Common.BackgroundJobs
{
    public interface IBackgroundJobService
    {
        Task<string> EnqueueAsync<T>(T job, DateTime? scheduleAt = null) where T : class;
        Task<string> ScheduleRecurringAsync<T>(string cronExpression, T job) where T : class;
        Task CancelAsync(string jobId);
        Task<JobStatus> GetJobStatusAsync(string jobId);
    }

    public enum JobStatus
    {
        Scheduled,
        Processing,
        Completed,
        Failed,
        Cancelled
    }

    public interface IJobHandler<in T> where T : class
    {
        Task ExecuteAsync(T job);
    }
} 