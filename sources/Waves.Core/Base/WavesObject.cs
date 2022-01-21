using System.Threading.Tasks;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Objects base class.
    /// </summary>
    public abstract class WavesObject :
        IWavesObject
    {
        /// <summary>
        /// Creates new instance os <see cref="WavesObject"/>.
        /// </summary>
        protected WavesObject()
        {
        }

        // /// <summary>
        // /// Creates new instance os <see cref="WavesObject"/>.
        // /// </summary>
        // /// <param name="configurationService">Instance of configuration service.</param>
        // protected WavesObject(IWavesConfigurationService configurationService)
        // {
        //     ConfigurationService = configurationService;
        // }

        /// <inheritdoc />
        public Task LoadConfigurationAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task SaveConfigurationAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
