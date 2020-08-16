using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of Waves's Module Library classes.
    /// </summary>
    public interface IModuleLibrary : IObject
    {
        /// <summary>
        ///     Gets description of module library.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Get manufacturer of module library.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        ///     Gets version of module library.
        /// </summary>
        Version Version { get; }

        /// <summary>
        ///     Gets collection of modules.
        /// </summary>
        ICollection<IModule> Modules { get; }

        /// <summary>
        ///     Updates modules collection.
        /// </summary>
        void UpdateModulesCollection();
    }
}