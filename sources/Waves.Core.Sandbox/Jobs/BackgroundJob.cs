using Microsoft.Extensions.Logging;
using Quartz;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;

namespace Waves.Core.Sandbox.Jobs;

/// <summary>
/// Sample background job.
/// </summary>
[WavesPlugin(typeof(BackgroundJob))]
public class BackgroundJob : WavesJob
{
    private readonly ILogger<BackgroundJob> _logger;

    /// <summary>
    /// Creates new instance of <see cref="BackgroundJob"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public BackgroundJob(ILogger<BackgroundJob> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public override Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("*** Task executed");
        return Task.CompletedTask;
    }
}
