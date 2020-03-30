using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Fluid.Core.Base.Interfaces
{
    /// <summary>
    /// Interface of module classes.
    /// </summary>
    public interface IModule : IObject, ICloneable, IDisposable, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets whether the module is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets icon of the module.
        /// </summary>
        string Icon { get; }

        /// <summary>
        ///     Gets description of the module.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets manufacturer of the module.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        ///     Gets version of the module.
        /// </summary>
        Version Version { get; }

        /// <summary>
        ///     Gets module's configuration.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        ///     Gets input points of the module.
        /// </summary>
        ICollection<IEntryPoint> Inputs { get; }

        /// <summary>
        ///     Gets output points of the module.
        /// </summary>
        ICollection<IEntryPoint> Outputs { get; }

        /// <summary>
        ///     Initializes module.
        /// </summary>
        void Initialize();
    }
}