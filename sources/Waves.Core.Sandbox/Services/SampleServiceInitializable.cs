using Microsoft.Extensions.Logging;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;

namespace Waves.Core.Sandbox.Services;

/// <summary>
/// Initializable sample service.
/// </summary>
[WavesPlugin(typeof(SampleServiceInitializable))]
public class SampleServiceInitializable : WavesPluginInitializable
{
    private readonly ILogger<SampleServiceInitializable> _logger;

    /// <summary>
    /// Creates new instance of <see cref="SampleServiceInitializable"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public SampleServiceInitializable(ILogger<SampleServiceInitializable> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public override Task InitializeAsync()
    {
        _logger.LogInformation("Initialization completed!");
        return Task.CompletedTask;
    }
}
