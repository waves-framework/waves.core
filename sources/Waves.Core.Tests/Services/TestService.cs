using Microsoft.Extensions.Logging;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;

namespace Waves.Core.Tests.Services;

/// <summary>
/// Sample service.
/// </summary>
[WavesPlugin(typeof(TestService))]
public class TestService : WavesPlugin
{
    /// <summary>
    /// Creates new instance of <see cref="TestService"/>.
    /// </summary>
    public TestService()
    {
    }
}
