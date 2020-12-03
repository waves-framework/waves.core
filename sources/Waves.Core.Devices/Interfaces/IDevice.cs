using System;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Devices.Interfaces
{
    /// <summary>
    ///     Interface for devices instances.
    /// </summary>
    public interface IDevice : IWavesModule
    {
        /// <summary>
        ///     Gets whether device is opened.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        ///     Gets whether device is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        ///     Device opened event.
        /// </summary>
        event EventHandler DeviceOpened;

        /// <summary>
        ///     Device closed event.
        /// </summary>
        event EventHandler DeviceClosed;

        /// <summary>
        ///     Device started event.
        /// </summary>
        event EventHandler DeviceStarted;

        /// <summary>
        ///     Device stopped event.
        /// </summary>
        event EventHandler DeviceStopped;

        /// <summary>
        ///     Opens device.
        /// </summary>
        void Open();

        /// <summary>
        ///     Closes device.
        /// </summary>
        void Close();

        /// <summary>
        ///     Starts device.
        /// </summary>
        void Start();

        /// <summary>
        ///     Stops device.
        /// </summary>
        void Stop();
    }
}