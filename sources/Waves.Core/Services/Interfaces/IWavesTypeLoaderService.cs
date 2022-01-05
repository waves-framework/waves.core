using System;
using System.Collections.Generic;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Services.Interfaces
{
    /// <summary>
    /// Type loader service.
    /// </summary>
    public interface IWavesTypeLoaderService : IWavesPluginInitializable
    {
        /// <summary>
        /// Event that fires when types loaded.
        /// </summary>
        event EventHandler<ICollection<Type>> TypesLoaded;
    }
}
