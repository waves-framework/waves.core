using System;
using Fluid.Core.Base;
using Fluid.Core.Devices.Interfaces;

namespace Fluid.Core.Devices
{
    public abstract class Device : Module, IDevice
    {
        private bool _isOpen;
        private bool _isRunning;

        /// <inheritdoc />
        public bool IsOpen
        {
            get => _isOpen;
            private set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (value == _isRunning) return;
                _isRunning = value;
                OnPropertyChanged();
            }
        }

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
        ///     Уведомление об открытии устройства.
        /// </summary>
        protected virtual void OnDeviceOpened()
        {
            DeviceOpened?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Уведомление о закрытии устройства.
        /// </summary>
        protected virtual void OnDeviceClosed()
        {
            DeviceClosed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Уведомление о запуске устройства.
        /// </summary>
        protected virtual void OnDeviceStarted()
        {
            DeviceStarted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Уведомление об остановке устройства.
        /// </summary>
        protected virtual void OnDeviceStopped()
        {
            DeviceStopped?.Invoke(this, EventArgs.Empty);
        }
    }
}