using System;
using System.Collections.Generic;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Services.Interfaces
{
    /// <summary>
    ///     Interface for module service classes.
    /// </summary>
    public interface IModuleService : IService
    {
        /// <summary>
        ///     Gets modules directory paths.
        /// </summary>
        List<string> ModulesPaths { get; }

        /// <summary>
        ///     Gets native libraries paths.
        /// </summary>
        List<string> NativeLibrariesPaths { get; }

        /// <summary>
        ///     Gets all of loaded libraries.
        /// </summary>
        IEnumerable<IModuleLibrary> Libraries { get; }

        /// <summary>
        ///     Gets all of loaded modules.
        /// </summary>
        List<IModule> Modules { get; }

        /// <summary>
        ///     List of loaded native libraries names.
        /// </summary>
        List<string> NativeLibrariesNames { get; }

        /// <summary>
        ///     Gets module by ID.
        /// </summary>
        /// <param name="id">ID.</param>
        IModule GetModule(string id);

        /// <summary>
        ///     Adds modules directory path.
        /// </summary>
        /// <param name="path">Path.</param>
        void AddModulePath(string path);

        /// <summary>
        ///     Adds native libraries directory path.
        /// </summary>
        /// <param name="path">Path.</param>
        void AddNativeLibraryPath(string path);

        /// <summary>
        ///     Removes modules directory path.
        /// </summary>
        /// <param name="path">Path.</param>
        void RemoveModulePath(string path);

        /// <summary>
        ///     Removes native libraries directory path.
        /// </summary>
        /// <param name="path">Path.</param>
        void RemoveNativeLibraryPath(string path);

        /// <summary>
        ///     Updates libraries collection.
        /// </summary>
        void UpdateLibraries();

        /// <summary>
        ///     Event for modules collection updated.
        /// </summary>
        event EventHandler ModulesUpdated;
    }
}