using System.Collections.Generic;
using System.Threading.Tasks;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Plugins.Services.Interfaces
{
    /// <summary>
    ///     Interface for native library loader service.
    /// </summary>
    public interface IWavesNativeLibraryService : IWavesService
    {
        /// <summary>
        ///     Get loading paths.
        /// </summary>
        List<string> Paths { get; }

        /// <summary>
        ///     Gets loading libraries names.
        /// </summary>
        List<string> Names { get; }

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
        ///     Updates loaded libraries.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateAsync();
    }
}
