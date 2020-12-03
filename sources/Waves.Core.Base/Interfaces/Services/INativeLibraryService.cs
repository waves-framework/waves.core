using System;
using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces.Services
{
    /// <summary>
    /// Interface for native library loader service.
    /// </summary>
    public interface INativeLibraryService : IWavesService
    {
        /// <summary>
        /// Get loading paths.
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
        void AddPath(string path);

        /// <summary>
        ///     Removes directory to loading from.
        /// </summary>
        /// <param name="path">Path.</param>
        void RemovePath(string path);

        /// <summary>
        /// Updates loaded libraries.
        /// </summary>
        void Update();
    }
}