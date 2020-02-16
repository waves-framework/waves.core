using System;
using System.Collections.Generic;
using Fluid.Core.Base.EventArgs;
using Fluid.Core.Devices.Interfaces.Input.ADC;

namespace Fluid.Core.Services.Interfaces
{
    public interface IAdcService : IService
    {
        /// <summary>
        ///     Запущены ли устройства.
        /// </summary>
        bool IsDevicesRunning { get; }

        /// <summary>
        ///     Общее количество каналов.
        /// </summary>
        short NumberOfChannels { get; }

        /// <summary>
        ///     Список устройств.
        /// </summary>
        List<IAdc> Devices { get; }

        /// <summary>
        ///     Список выбранных устройств.
        /// </summary>
        List<IAdc> SelectedDevices { get; }

        /// <summary>
        ///     Список каналов.
        /// </summary>
        List<IAdcEntryPoint> Channels { get; }

        /// <summary>
        ///     Список выбранных каналов.
        /// </summary>
        List<IAdcEntryPoint> SelectedChannels { get; }

        /// <summary>
        ///     Запуск устройств.
        /// </summary>
        void StartDevices();

        /// <summary>
        ///     Остановка устройств.
        /// </summary>
        void StopDevices();

        /// <summary>
        ///     Выбирает устройство.
        /// </summary>
        /// <param name="device"></param>
        void SelectDevice(IAdc device);

        /// <summary>
        ///     Выбирает канал.
        /// </summary>
        /// <param name="channel"></param>
        void SelectChannel(IAdcEntryPoint channel);

        /// <summary>
        ///     Снимает выбор с устройства.
        /// </summary>
        /// <param name="device"></param>
        void UnselectDevice(IAdc device);

        /// <summary>
        ///     Снимает выбор с канала.
        /// </summary>
        /// <param name="channel"></param>
        void UnselectChannel(IAdcEntryPoint channel);

        /// <summary>
        ///     Событие приема данных с канала.
        /// </summary>
        event EventHandler<DataReceivedEventArgs> ChannelDataReceived;

        /// <summary>
        ///     Обновление устройств.
        /// </summary>
        event EventHandler DevicesUpdated;
    }
}