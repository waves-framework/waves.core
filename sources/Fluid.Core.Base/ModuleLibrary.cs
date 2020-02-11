using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fluid.Core.Base.Annotations;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    public abstract class ModuleLibrary : IModuleLibrary
    {
        private ICollection<IModule> _modules = new List<IModule>();

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract string Description { get; }

        /// <inheritdoc />
        public abstract string Manufacturer { get; }

        /// <inheritdoc />
        public abstract Version Version { get; }

        /// <inheritdoc />
        public ICollection<IModule> Modules
        {
            get => _modules;
            private set
            {
                if (Equals(value, _modules)) return;
                _modules = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public abstract void UpdateModulesList();

        /// <inheritdoc />
        public event EventHandler<IMessage> MessageReceived;

        /// <summary>
        ///     Уведомление об изменении параметров.
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Уведомление об отправке системного сообщения.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMessageReceived(IMessage e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}