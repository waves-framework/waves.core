using System;
using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces.Services
{
    /// <summary>
    /// Loader service using MEF.
    /// </summary>
    public interface IMefLoaderService<T>: IWavesService where T: IWavesObject
    {
        /// <summary>
        ///     Event for objects collection updated.
        /// </summary>
        event EventHandler ObjectsUpdated;

        /// <summary>
        /// Get loading paths.
        /// </summary>
        List<string> Paths { get; }

        /// <summary>
        ///     Gets loading objects.
        /// </summary>
        IEnumerable<T> Objects { get; }

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
        /// Updates collection of objects.
        /// </summary>
        void Update();
    }
}