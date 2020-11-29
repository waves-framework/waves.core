using System;
using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Base abstract application class.
    /// </summary>
    public abstract class WavesApplication : WavesObject, IWavesApplication
    {
        /// <inheritdoc />
        public event ApplicationsActionsUpdatedEventHandler ActionsUpdated;

        /// <inheritdoc />
        [Reactive]
        public bool IsInitialized { get; set; }

        /// <inheritdoc />
        public abstract IWavesColor IconBackgroundColor { get; }

        /// <inheritdoc />
        public abstract IWavesColor IconForegroundColor { get; }

        /// <inheritdoc />
        [Reactive]
        public IWavesConfiguration Configuration { get; internal set; } = new WavesConfiguration();

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
        public ICollection<IWavesApplicationAction> Actions { get; internal set; } = new List<IWavesApplicationAction>();

        /// <inheritdoc />
        public abstract void Initialize();

        /// <inheritdoc />
        public abstract void SaveConfiguration();

        /// <inheritdoc />
        public abstract override void Dispose();

        /// <summary>
        /// Notifies when applications actions updated.
        /// </summary>
        /// <param name="args">Args.</param>
        protected virtual void OnActionsUpdated(ApplicationActionsUpdatedEventArgs args)
        {
            ActionsUpdated?.Invoke(this, args);
        }
    }
}