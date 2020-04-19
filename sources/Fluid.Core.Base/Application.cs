using System;
using System.Collections.Generic;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    ///     Base abstract application class.
    /// </summary>
    public abstract class Application : Object, IApplication
    {
        /// <inheritdoc />
        public bool IsInitialized { get; set; }

        /// <inheritdoc />
        public abstract IColor IconBackgroundColor { get; }

        /// <inheritdoc />
        public abstract IColor IconForegroundColor { get; }

        /// <inheritdoc />
        public IConfiguration Configuration { get; } = new Configuration();

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
        public ICollection<IApplicationAction> Actions { get; } = new List<IApplicationAction>();

        /// <inheritdoc />
        public event EventHandler ActionsUpdated;

        /// <inheritdoc />
        public abstract void Initialize();

        /// <inheritdoc />
        public abstract void SaveConfiguration();

        /// <inheritdoc />
        public abstract void Dispose();

        /// <summary>
        ///     Notifies when actions collection changed.
        /// </summary>
        protected virtual void OnActionsUpdated()
        {
            ActionsUpdated?.Invoke(this, System.EventArgs.Empty);
        }
    }
}