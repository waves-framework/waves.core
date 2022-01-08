using System.Collections.Generic;
using System.Threading.Tasks;

namespace Waves.Core.Services.Interfaces
{
    /// <summary>
    ///     Interface for native library loader service.
    /// </summary>
    public interface IWavesNativeLibraryService
    {
        /// <summary>
        ///     Gets loading libraries names.
        /// </summary>
        List<string> Names { get; }

        /// <summary>
        ///     Updates loaded libraries.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateAsync();
    }
}
