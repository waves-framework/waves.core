using System;
using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of application classes.
    /// </summary>
    public interface IApplication : IObject, IDisposable
    {
        /// <summary>
        ///     Gets whether the application is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        ///     Gets icon's background color.
        /// </summary>
        IColor IconBackgroundColor { get; }

        /// <summary>
        ///     Gets icon's foreground color.
        /// </summary>
        IColor IconForegroundColor { get; }

        /// <summary>
        ///     Gets application's configuration.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        ///     Gets application's icon.
        /// </summary>
        string Icon { get; }

        /// <summary>
        ///     Gets description.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets manufacturer.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        ///     Gets version.
        /// </summary>
        Version Version { get; }

        /// <summary>
        ///     Gets collection of available actions.
        /// </summary>
        ICollection<IApplicationAction> Actions { get; }

        /// <summary>
        ///     Event for actions updated.
        /// </summary>
        event EventHandler ActionsUpdated;

        /// <summary>
        ///     Initializes application.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Saves configuration.
        /// </summary>
        void SaveConfiguration();
    }
}