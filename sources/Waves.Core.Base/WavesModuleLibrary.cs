using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Module library base class.
    /// </summary>
    public abstract class WavesModuleLibrary : WavesObject, IWavesModuleLibrary
    {
        /// <inheritdoc />
        public abstract override string Name { get; }

        /// <inheritdoc />
        public abstract string Description { get; }

        /// <inheritdoc />
        public abstract string Manufacturer { get; }

        /// <inheritdoc />
        public abstract Version Version { get; }

        /// <inheritdoc />
        [Reactive]
        public ICollection<IWavesModule> Modules { get; internal set; } = new List<IWavesModule>();

        /// <inheritdoc />
        public abstract void UpdateModulesCollection();
    }
}