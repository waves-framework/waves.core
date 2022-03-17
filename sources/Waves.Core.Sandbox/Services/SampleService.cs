using Microsoft.Extensions.Logging;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;

namespace Waves.Core.Sandbox.Services;

/// <summary>
/// Sample service.
/// </summary>
[WavesPlugin(typeof(SampleService))]
public class SampleService : WavesPlugin
{
    private readonly ILogger<SampleService> _logger;

    /// <summary>
    /// Creates new instance of <see cref="SampleService"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public SampleService(ILogger<SampleService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Run sample method.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task Run()
    {
        _logger.LogInformation("Hello from sample service!");
        return Task.CompletedTask;
    }
}
