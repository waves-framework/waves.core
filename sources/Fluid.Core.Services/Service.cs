using System;
using Fluid.Core.Base;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Services.Interfaces;
using Object = Fluid.Core.Base.Object;

namespace Fluid.Core.Services
{
    /// <summary>
    /// Service base class.
    /// </summary>
    public abstract class Service : Object, IService
    {
        /// <inheritdoc />
        public abstract override Guid Id { get; }

        /// <inheritdoc />
        public abstract override string Name { get; }

        /// <inheritdoc />
        public bool IsInitialized { get; set; } = false;

        /// <inheritdoc />
        public abstract void Initialize();

        /// <inheritdoc />
        public abstract void LoadConfiguration(IConfiguration configuration);

        /// <inheritdoc />
        public abstract void SaveConfiguration(IConfiguration configuration);

        /// <inheritdoc />
        public abstract void Dispose();

        /// <summary>
        /// Loads property value from configuration.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="configuration">Configuration instance.</param>
        /// <param name="key">Property key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Value.</returns>
        public static T LoadConfigurationValue<T>(IConfiguration configuration, string key, T defaultValue)
        {
            try
            {
                if (configuration == null)
                    throw new Exception("Configuration is null.\r\n");

                if (configuration.Contains(key))
                    return (T)configuration.GetPropertyValue(key);

                configuration.AddProperty(key, defaultValue, false);

                return (T) defaultValue;
            }
            catch (Exception e)
            {
                throw new Exception("Error loading configuration value.\r\n" + e.Message);
            }
        }
    }
}