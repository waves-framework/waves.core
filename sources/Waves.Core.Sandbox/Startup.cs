using Microsoft.Extensions.DependencyInjection;
using Waves.Core.Base;

namespace Waves.Core.Sandbox;

/// <summary>
/// Startup class.
/// </summary>
public class Startup : WavesStartup
{
    /// <inheritdoc />
    protected override void ConfigureAdditionalServices(IServiceCollection services)
    {
        // TODO: add your services here.
        // services.AddTransient<ISampleService, SampleService>();
    }
}
