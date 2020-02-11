
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Base
{
    public abstract class Application : Object, IApplication
    {
        private bool _isInitialized;

        private IConfiguration _configuration = new Configuration();

        private ICollection<IApplicationAction> _actions = new List<IApplicationAction>();

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
        public abstract IColor IconBackgroundColor { get; }

        /// <inheritdoc />
        public abstract IColor IconForegroundColor { get; }

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
        public abstract override Guid Id { get; }

        /// <inheritdoc />
        public abstract override string Name { get; }

        /// <inheritdoc />
        public abstract string Icon { get; }

        /// <inheritdoc />
        public abstract string Description { get; }

        /// <inheritdoc />
        public abstract string Manufacturer { get; }

        /// <inheritdoc />
        public abstract Version Version { get; }

        /// <inheritdoc />
        public ICollection<IApplicationAction> Actions
        {
            get => _actions;
            private set
            {
                if (Equals(value, _actions)) return;
                _actions = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public event System.EventHandler<IMessage> MessageReceived;

        /// <inheritdoc />
        public event EventHandler ActionsUpdated;

        /// <inheritdoc />
        public abstract void Initialize();

        /// <inheritdoc />
        public abstract void SaveConfiguration();

        /// <inheritdoc />
        public abstract void Dispose();

        /// <summary>
        ///     Уведомления об обнолении списка доступных действий.
        /// </summary>
        protected virtual void OnActionsUpdated()
        {
            ActionsUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Уведомление об отправке системного сообщения.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        protected virtual void OnMessageReceived(IMessage message)
        {
            MessageReceived?.Invoke(this, message);
        }
    }
}