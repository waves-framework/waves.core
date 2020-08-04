using System;
using System.Collections.Generic;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Services.Interfaces
{
    /// <summary>
    ///     Interface for Application service classes.
    /// </summary>
    public interface IApplicationService : IService
    {
        /// <summary>
        ///     Gets paths to application directories.
        /// </summary>
        List<string> Paths { get; }

        /// <summary>
        ///     Gets applications.
        /// </summary>
        IEnumerable<IApplication> Applications { get; }

        /// <summary>
        ///     Gets all of application actions.
        /// </summary>
        ICollection<IApplicationAction> ApplicationActions { get; }

        /// <summary>
        ///     Adds path to applications directory.
        /// </summary>
        /// <param name="path"></param>
        void AddPath(string path);

        /// <summary>
        ///     Removes path to applications directory.
        /// </summary>
        /// <param name="path"></param>
        void RemovePath(string path);

        /// <summary>
        ///     Updates applications collection.
        /// </summary>
        void UpdateApplicationsCollection();

        /// <summary>
        ///     Event for applications collection updated.
        /// </summary>
        event EventHandler ApplicationsUpdated;

        /// <summary>
        ///     Event for applications actions updated.
        /// </summary>
        event EventHandler ApplicationsActionsUpdated;
    }
}