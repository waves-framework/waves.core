using System;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Extensions;
using Waves.Core.Base.Interfaces;
using Waves.Core.Plugins.Services.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    /// Configurable plugin abstraction.
    /// </summary>
    public abstract class WavesConfigurablePlugin : 
        WavesConfigurableObject,
        IWavesPlugin
    {
        /// <summary>
        /// Creates new instance of <see cref="WavesConfigurableObject"/>.
        /// </summary>
        /// <param name="configurationService">Instance of configuration service.</param>
        protected WavesConfigurablePlugin(
            IWavesConfigurationService configurationService)
            : base(configurationService)
        {
        }

        /// <inheritdoc />
        [Reactive]
        public bool IsInitialized { get; protected set; }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public virtual async Task InitializeAsync()
        {
            if (IsInitialized)
            {
                return;
            }

            await LoadConfigurationAsync();
        }

        /// <inheritdoc />
        public override async Task SaveConfigurationAsync()
        {
            await base.SaveConfigurationAsync();

            Configuration.AddProperty("ParentId", this.GetGuid());
            Configuration.AddProperty("ParentName", this.GetPluginTypeName());
        }

        /// <summary>
        ///     Disposes object.
        /// </summary>
        /// <param name="disposing">Set
        ///     <value>true</value>
        ///     if you need to release managed and unmanaged resources. Set
        ///     <value>false</value>
        ///     if need to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(
            bool disposing)
        {
        }
    }
}
