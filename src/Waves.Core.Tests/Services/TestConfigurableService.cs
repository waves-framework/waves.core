using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;

namespace Waves.Core.Tests.Services;

/// <summary>
/// Sample configurable service.
/// </summary>
[WavesPlugin(typeof(TestConfigurableService))]
public class TestConfigurableService : WavesConfigurablePlugin
{
    /// <summary>
    /// Creates new instance of <see cref="TestConfigurableService"/>.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    /// <param name="logger">Logger.</param>
    public TestConfigurableService(
        IConfiguration configuration,
        ILogger<TestConfigurableService> logger)
        : base(configuration, logger)
    {
    }

    /// <summary>
    /// Gets or sets test value.
    /// </summary>
    [WavesProperty]
    public int TestValue { get; set; }
}
