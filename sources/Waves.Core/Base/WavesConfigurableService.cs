using Waves.Core.Base.Interfaces;
using Waves.Core.Plugins.Services.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    /// Abstraction for configurable service.
    /// </summary>
    public abstract class WavesConfigurableService : WavesConfigurablePlugin, IWavesConfigurableService
    {
        /// <summary>
        /// Creates new instance of <see cref="WavesConfigurableObject"/>.
        /// </summary>
        /// <param name="configurationService">Instance of configuration service.</param>
        protected WavesConfigurableService(
            IWavesConfigurationService configurationService)
            : base(configurationService)
        {
        }
    }
}
