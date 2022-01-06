using Microsoft.Extensions.Logging;
using Waves.Core.Sandbox.Services.Interfaces;
using ILogger = Splat.ILogger;

namespace Waves.Core.Sandbox.Services;

/// <summary>
/// Sample service.
/// </summary>
public class SampleService : ISampleService
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

    /// <inheritdoc />
    public void SampleMethod()
    {
        _logger.LogInformation("Sample method invoked.");
    }
}
