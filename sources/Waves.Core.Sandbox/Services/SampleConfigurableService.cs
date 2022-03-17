using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;
using Waves.Core.Services;

namespace Waves.Core.Sandbox.Services;

/// <summary>
/// Sample configurable service.
/// </summary>
[WavesPlugin(typeof(SampleConfigurableService))]
public class SampleConfigurableService : WavesConfigurablePlugin
{
    private int _testValue;

    /// <summary>
    /// Creates new instance of <see cref="SampleConfigurableService"/>.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    /// <param name="logger">Logger.</param>
    public SampleConfigurableService(
        IConfiguration configuration,
        ILogger<SampleConfigurableService> logger)
        : base(configuration, logger)
    {
    }

    /// <summary>
    /// Gets or sets test value.
    /// </summary>
    [WavesProperty]
    public int TestValue
    {
        get => _testValue;
        set => _testValue = value;
    }
}
