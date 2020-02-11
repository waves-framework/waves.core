using System;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Devices.Interfaces
{
    public interface IDevice : IModule
    {
        /// <summary>
        ///     Открыто ли устройство.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        ///     Запущено ли устройство.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        ///     Событие открытия устройства.
        /// </summary>
        event EventHandler DeviceOpened;

        /// <summary>
        ///     Событие закрытия устройства.
        /// </summary>
        event EventHandler DeviceClosed;

        /// <summary>
        ///     Событие запуска работы устройства.
        /// </summary>
        event EventHandler DeviceStarted;

        /// <summary>
        ///     Событие остановки работы устройства.
        /// </summary>
        event EventHandler DeviceStopped;

        /// <summary>
        ///     Открытие устройства.
        /// </summary>
        void Open();

        /// <summary>
        ///     Закрытие устройства.
        /// </summary>
        void Close();

        /// <summary>
        ///     Запуск работы устройства.
        /// </summary>
        void Start();

        /// <summary>
        ///     Остановка работы устройства.
        /// </summary>
        void Stop();
    }
}