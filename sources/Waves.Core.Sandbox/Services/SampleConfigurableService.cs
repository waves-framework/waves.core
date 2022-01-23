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
    /// <summary>
    /// Creates new instance of <see cref="SampleConfigurableService"/>.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    public SampleConfigurableService(
        IConfiguration configuration)
        : base(configuration)
    {
    }
}
