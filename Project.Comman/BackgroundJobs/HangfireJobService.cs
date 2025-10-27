using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Storage;

namespace Ettad.Shared.Common.BackgroundJobs
{
    public class HangfireJobService : IBackgroundJobService
    {
        public Task<string> EnqueueAsync<T>(T job, DateTime? scheduleAt = null) where T : class
        {
            string jobId;
            if (scheduleAt.HasValue)
            {
                jobId = BackgroundJob.Schedule<IJobHandler<T>>(
                    handler => handler.ExecuteAsync(job),
                    scheduleAt.Value);
            }
            else
            {
                jobId = BackgroundJob.Enqueue<IJobHandler<T>>(
                    handler => handler.ExecuteAsync(job));
            }

            return Task.FromResult(jobId);
        }

        public Task<string> ScheduleRecurringAsync<T>(string cronExpression, T job) where T : class
        {
             RecurringJob.AddOrUpdate<IJobHandler<T>>(
                typeof(T).Name,
                handler => handler.ExecuteAsync(job),
                cronExpression);

            return Task.FromResult("0");
        }

        public Task CancelAsync(string jobId)
        {
            BackgroundJob.Delete(jobId);
            return Task.CompletedTask;
        }

        public Task<JobStatus> GetJobStatusAsync(string jobId)
        {
            using var connection = JobStorage.Current.GetConnection();
            var job = connection.GetJobData(jobId);

            if (job == null)
                return Task.FromResult(JobStatus.Failed);

            var status = job.State switch
            {
                "Scheduled" => JobStatus.Scheduled,
                "Processing" => JobStatus.Processing,
                "Succeeded" => JobStatus.Completed,
                "Failed" => JobStatus.Failed,
                "Deleted" => JobStatus.Cancelled,
                _ => JobStatus.Failed
            };

            return Task.FromResult(status);
        }
    }
} 