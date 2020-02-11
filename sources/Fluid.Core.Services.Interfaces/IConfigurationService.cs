using System;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Services.Interfaces
{
    public interface IConfigurationService : IService 
    {
        /// <summary>
        /// Путь к файлу конфигурации.
        /// </summary>
        string ConfigurationFileName { get; }

        /// <summary>
        /// Конфигурация.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Сохранение.
        /// </summary>
        void Save();

        /// <summary>
        /// Загрузка.
        /// </summary>
        void Load(string fileName);

        /// <summary>
        /// Событие обновление конфигурации.
        /// </summary>
        event EventHandler ConfigurationUpdated;
    }
}
