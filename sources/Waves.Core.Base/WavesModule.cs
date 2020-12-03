using System;
using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Module base class.
    /// </summary>
    [Serializable]
    public abstract class WavesModule : WavesObject, IWavesModule
    {
        /// <inheritdoc />
        public bool IsInitialized { get; set; }

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
        [Reactive]
        public IWavesConfiguration Configuration { get; set; } = new WavesConfiguration();

        /// <inheritdoc />
        [Reactive]
        public ICollection<IWavesEntryPoint> Inputs { get; set; } = new List<IWavesEntryPoint>();

        /// <inheritdoc />
        [Reactive]
        public ICollection<IWavesEntryPoint> Outputs { get; set; } = new List<IWavesEntryPoint>();

        /// <inheritdoc />
        public abstract void Initialize();

        /// <inheritdoc />
        public abstract object Clone();

        /// <inheritdoc />
        public abstract override void Dispose();
    }
}