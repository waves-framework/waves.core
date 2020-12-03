using System;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Service base class.
    /// </summary>
    public abstract class WavesService : WavesObject, IWavesService
    {
        /// <inheritdoc />
        public abstract override Guid Id { get; }

        /// <inheritdoc />
        public abstract override string Name { get; }

        /// <inheritdoc />
        [Reactive]
        public bool IsInitialized { get; set; } = false;

        /// <summary>
        ///     Gets instance of Core.
        /// </summary>
        protected IWavesCore Core { get; set; }

        /// <inheritdoc />
        public abstract void Initialize(IWavesCore core);

        /// <inheritdoc />
        public abstract void LoadConfiguration();

        /// <inheritdoc />
        public abstract void SaveConfiguration();

        /// <inheritdoc />
        public abstract override void Dispose();

        /// <summary>
        ///     Loads property value from configuration.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="configuration">Configuration instance.</param>
        /// <param name="key">Property key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Value.</returns>
        protected static T LoadConfigurationValue<T>(IWavesConfiguration configuration, string key, T defaultValue)
        {
            try
            {
                if (configuration == null)
                    throw new Exception("Configuration is null.\r\n");

                if (configuration.Contains(key))
                    return (T) configuration.GetPropertyValue(key);

                configuration.AddProperty(key, defaultValue, false);

                return defaultValue;
            }
            catch (Exception e)
            {
                throw new Exception("Error loading configuration value.\r\n" + e.Message);
            }
        }
    }
}