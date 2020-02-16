using System;
using System.Collections.Generic;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Services.Interfaces
{
    public interface ISensorService : IService
    {
        /// <summary>
        ///     Коллекция датчиков.
        /// </summary>
        List<ISensor> Sensors { get; }

        /// <summary>
        ///     Добавление датчика.
        /// </summary>
        /// <param name="sensor">Датчик.</param>
        void AddSensor(ISensor sensor);

        /// <summary>
        ///     Удаление датчика.
        /// </summary>
        /// <param name="sensor">Датчик.</param>
        void RemoveSensor(ISensor sensor);

        /// <summary>
        ///     Обновление датчика.
        /// </summary>
        /// <param name="sensor">Датчик.</param>
        void UpdateSensor(ISensor sensor);

        /// <summary>
        ///     Событие обновление списка датчиков.
        /// </summary>
        event EventHandler SensorsUpdated;
    }
}