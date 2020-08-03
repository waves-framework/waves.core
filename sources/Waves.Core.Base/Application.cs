using System;
using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Base abstract application class.
    /// </summary>
    public abstract class Application : Object, IApplication
    {
        /// <inheritdoc />
        [Reactive]
        public bool IsInitialized { get; set; }

        /// <inheritdoc />
        public abstract IColor IconBackgroundColor { get; }

        /// <inheritdoc />
        public abstract IColor IconForegroundColor { get; }

        /// <inheritdoc />
        [Reactive]
        public IConfiguration Configuration { get; internal set; } = new Configuration();

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
        [Reactive]
        public ICollection<IApplicationAction> Actions { get; internal set; } = new List<IApplicationAction>();

        /// <inheritdoc />
        public event EventHandler ActionsUpdated;

        /// <inheritdoc />
        public abstract void Initialize();

        /// <inheritdoc />
        public abstract void SaveConfiguration();

        /// <inheritdoc />
        public abstract override void Dispose();

        /// <summary>
        ///     Notifies when actions collection changed.
        /// </summary>
        protected virtual void OnActionsUpdated()
        {
            ActionsUpdated?.Invoke(this, System.EventArgs.Empty);
        }
    }
}