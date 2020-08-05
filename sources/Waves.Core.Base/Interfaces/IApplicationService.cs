using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface for Application service classes.
    /// </summary>
    public interface IApplicationService : IMefLoaderService<IApplication>
    {
        /// <summary>
        ///     Gets all of application actions.
        /// </summary>
        ICollection<IApplicationAction> ApplicationActions { get; }

        /// <summary>
        ///     Event for applications actions updated.
        /// </summary>
        event ApplicationsActionsUpdatedEventHandler ApplicationsActionsUpdated;
    }
}