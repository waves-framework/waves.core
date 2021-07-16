using System;
using System.Collections.Generic;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Plugins.Services.Interfaces
{
    /// <summary>
    /// Type loader service.
    /// </summary>
    public interface IWavesTypeLoaderService : IWavesService
    {
        /// <summary>
        /// Gets collection of loaded types.
        /// </summary>
        ICollection<Type> Types { get; }
    }
}
