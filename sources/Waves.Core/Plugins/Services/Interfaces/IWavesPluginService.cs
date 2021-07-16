using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Plugins.Services.Interfaces
{
    /// <summary>
    /// Loader service using MEF.
    /// </summary>
    /// <typeparam name="T">Type of loading objects.</typeparam>
    public interface IWavesPluginService<out T> : IWavesService
        where T : IWavesObject
    {
        /// <summary>
        ///     Event for objects collection updated.
        /// </summary>
        event EventHandler ObjectsUpdated;

        /// <summary>
        ///     Get loading paths.
        /// </summary>
        List<string> Paths { get; }

        /// <summary>
        ///     Gets loading objects.
        /// </summary>
        IEnumerable<T> Plugins { get; }

        /// <summary>
        ///     Adds directory to loading from.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddPathAsync(
            string path);

        /// <summary>
        ///     Removes directory to loading from.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemovePathAsync(
            string path);

        /// <summary>
        ///     Updates collection of objects.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateAsync();
    }
}
