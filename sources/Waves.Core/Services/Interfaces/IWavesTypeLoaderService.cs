using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Waves.Core.Services.Interfaces
{
    /// <summary>
    /// Type loader service.
    /// </summary>
    /// <typeparam name="T">Attribute type.</typeparam>
    public interface IWavesTypeLoaderService<T>
    {
        /// <summary>
        /// Gets loaded types.
        /// </summary>
        Dictionary<Type, T> Types { get; }

        /// <summary>
        /// Updates types async.
        /// </summary>
        /// <returns>Task.</returns>
        Task UpdateTypesAsync();
    }
}
