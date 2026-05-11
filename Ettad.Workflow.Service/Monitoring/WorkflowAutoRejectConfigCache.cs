using Ettad.EntityFramework.DataBaseContext;
using Ettad.Workflows.Service.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Monitoring;

public class WorkflowAutoRejectConfigCache : IWorkflowAutoRejectConfigCache
{
    private const int CacheTtlMinutes = 30;
    private const string CacheKeyPrefix = "auto-reject-trigger-";

    private readonly IServiceProvider _serviceProvider;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<WorkflowAutoRejectConfigCache> _logger;

    public WorkflowAutoRejectConfigCache(
        IServiceProvider serviceProvider,
        IMemoryCache memoryCache,
        ILogger<WorkflowAutoRejectConfigCache> logger)
    {
        _serviceProvider = serviceProvider;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<WorkflowAutoRejectTriggerConfig> GetConfigAsync(long workflowId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{CacheKeyPrefix}{workflowId}";

        if (_memoryCache.TryGetValue(cacheKey, out WorkflowAutoRejectTriggerConfig? cached) && cached != null)
            return cached;

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var trigger = await dbContext.WorkflowAutoRejectTriggers
            .AsNoTracking()
            .Include(t => t.TriggerRoles)
            .Include(t => t.TriggerSteps)
            .FirstOrDefaultAsync(t => t.WorkflowId == workflowId, cancellationToken)
            .ConfigureAwait(false);

        var config = WorkflowAutoRejectConfigParser.Parse(trigger);

        _memoryCache.Set(
            cacheKey,
            config,
            new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheTtlMinutes) });

        _logger.LogDebug("Cached auto-reject config for workflow {WorkflowId}, mode: {Mode}", workflowId, config.Mode);

        return config;
    }

    public void InvalidateConfig(long workflowId)
    {
        _memoryCache.Remove($"{CacheKeyPrefix}{workflowId}");
        _logger.LogDebug("Invalidated auto-reject config for workflow {WorkflowId}", workflowId);
    }
}
