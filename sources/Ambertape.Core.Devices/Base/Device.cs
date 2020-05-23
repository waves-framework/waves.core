using System;
using Ambertape.Core.Base;
using Ambertape.Core.Devices.Interfaces;

namespace Ambertape.Core.Devices.Base
{
    /// <summary>
    ///     Abstract device base class.
    /// </summary>
    public abstract class Device : Module, IDevice
    {
        /// <inheritdoc />
        public bool IsOpen { get; set; }

        /// <inheritdoc />
        public bool IsRunning { get; set; }

        /// <inheritdoc />
        public event EventHandler DeviceOpened;

        /// <inheritdoc />
        public event EventHandler DeviceClosed;

        /// <inheritdoc />
        public event EventHandler DeviceStarted;

        /// <inheritdoc />
        public event EventHandler DeviceStopped;

        /// <inheritdoc />
        public abstract void Open();

        /// <inheritdoc />
        public abstract void Close();

        /// <inheritdoc />
        public abstract void Start();

        /// <inheritdoc />
        public abstract void Stop();

        /// <summary>
        ///     Notifies when device opened.
        /// </summary>
        protected virtual void OnDeviceOpened()
        {
            DeviceOpened?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Notifies when device closed.
        /// </summary>
        protected virtual void OnDeviceClosed()
        {
            DeviceClosed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Notifies when device started.
        /// </summary>
        protected virtual void OnDeviceStarted()
        {
            DeviceStarted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Notifies when device stopped.
        /// </summary>
        protected virtual void OnDeviceStopped()
        {
            DeviceStopped?.Invoke(this, EventArgs.Empty);
        }
    }
}