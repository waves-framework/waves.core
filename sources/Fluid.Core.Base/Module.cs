using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Base
{
    [Serializable]
    public abstract class Module : Object, IModule
    {
        private bool _isInitialized;

        private IConfiguration _configuration = new Configuration();

        private ICollection<IEntryPoint> _inputs = new List<IEntryPoint>();

        private ICollection<IEntryPoint> _outputs = new List<IEntryPoint>();
        
        /// <inheritdoc />
        public bool IsInitialized
        {
            get => _isInitialized;
            private set
            {
                if (value == _isInitialized) return;
                _isInitialized = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public abstract string Icon { get; }

        /// <inheritdoc />
        public abstract override string Name { get; }

        /// <inheritdoc />
        public abstract string Description { get; }

        /// <inheritdoc />
        public abstract override Guid Id { get; }

        /// <inheritdoc />
        public abstract string Manufacturer { get; }

        /// <inheritdoc />
        public abstract Version Version { get; }

        /// <inheritdoc />
        public IConfiguration Configuration
        {
            get => _configuration;
            private set
            {
                if (Equals(value, _configuration)) return;
                _configuration = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public ICollection<IEntryPoint> Inputs
        {
            get => _inputs;
            private set
            {
                if (Equals(value, _inputs)) return;
                _inputs = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public ICollection<IEntryPoint> Outputs
        {
            get => _outputs;
            private set
            {
                if (Equals(value, _outputs)) return;
                _outputs = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public event EventHandler<IMessage> MessageReceived;

        /// <inheritdoc />
        public abstract void Initialize();

        /// <inheritdoc />
        public abstract void Execute();

        /// <inheritdoc />
        public abstract object Clone();

        /// <inheritdoc />
        public abstract void Dispose();

        /// <summary>
        /// Получение хэш-кода.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 5;

            foreach (var property in Configuration.Properties)
                hashCode = hashCode * 31 + property.GetHashCode();

            foreach (var point in Inputs)
                hashCode = hashCode * 31 + point.GetHashCode();

            foreach (var point in Outputs)
                hashCode = hashCode * 31 + point.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Уведомление о приеме системного сообщения.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMessageReceived(IMessage e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}