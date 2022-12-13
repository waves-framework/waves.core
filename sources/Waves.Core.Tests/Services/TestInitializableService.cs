using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;

namespace Waves.Core.Tests.Services;

/// <summary>
/// Sample configurable service.
/// </summary>
[WavesPlugin(typeof(TestInitializableService))]
public class TestInitializableService : WavesInitializablePlugin
{
    /// <summary>
    /// Creates new instance of <see cref="TestInitializableService"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public TestInitializableService(ILogger<WavesInitializablePlugin> logger)
        : base(logger)
    {
    }

    /// <summary>
    /// Gets or sets test value.
    /// </summary>
    public int TestValue { get; set; }

    /// <inheritdoc />
    protected override Task RunInitializationAsync()
    {
        TestValue = 50;
        return base.RunInitializationAsync();
    }
}
