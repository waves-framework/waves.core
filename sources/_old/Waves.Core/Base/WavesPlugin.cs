using System;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Abstract base for plugins.
    /// </summary>
    public abstract class WavesPlugin : 
        WavesObject,
        IWavesPlugin
    {
        /// <inheritdoc />
        [Reactive]
        public bool IsInitialized { get; protected set; }

        /// <inheritdoc />
        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Disposes object.
        /// </summary>
        /// <param name="disposing">Set
        ///     <value>true</value>
        ///     if you need to release managed and unmanaged resources. Set
        ///     <value>false</value>
        ///     if need to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
