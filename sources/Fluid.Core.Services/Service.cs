using System;
using Fluid.Core.Base;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Services.Interfaces;
using Object = Fluid.Core.Base.Object;

namespace Fluid.Core.Services
{
    public abstract class Service : Object, IService
    {
        /// <inheritdoc />
        public abstract override Guid Id { get; }

        /// <inheritdoc />
        public abstract override string Name { get; }

        /// <summary>
        ///     Инициализирован ли сервис
        /// </summary>
        public bool IsInitialized { get; set; } = false;

        /// <summary>
        ///     Инициализация сервиса
        /// </summary>
        public abstract void Initialize();

        /// <inheritdoc />
        public abstract void LoadConfiguration(IConfiguration configuration);

        /// <inheritdoc />
        public abstract void SaveConfiguration(IConfiguration configuration);

        /// <inheritdoc />
        public abstract void Dispose();

        /// <summary>
        ///     Загружает значение из конфигурации.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T LoadConfigurationValue<T>(IConfiguration configuration, string key)
        {
            if (configuration.Contains(key))
                return (T) configuration.GetPropertyValue(key);

            configuration.AddProperty(new Property(key, default, false));

            return default;
        }
    }
}