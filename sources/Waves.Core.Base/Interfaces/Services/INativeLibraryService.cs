using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces.Services
{
    /// <summary>
    /// Interface for native library loader service.
    /// </summary>
    public interface INativeLibraryService : IService
    {
        /// <summary>
        ///     Gets native libraries paths.
        /// </summary>
        ICollection<string> Paths { get; }

        /// <summary>
        ///     List of loaded native libraries names.
        /// </summary>
        ICollection<string> Names { get; }

        /// <summary>
        ///     Adds directory path.
        /// </summary>
        /// <param name="path">Path.</param>
        void AddPath(string path);

        /// <summary>
        ///     Removes directory path.
        /// </summary>
        /// <param name="path">Path.</param>
        void RemovePath(string path);

        /// <summary>
        ///     Updates libraries collection.
        /// </summary>
        void Update();
    }
}