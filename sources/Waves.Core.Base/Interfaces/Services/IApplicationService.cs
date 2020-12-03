using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces.Services
{
    /// <summary>
    ///     Interface for Application service classes.
    /// </summary>
    public interface IApplicationService : IMefLoaderService<IWavesApplication>
    {
        /// <summary>
        ///     Gets all of application actions.
        /// </summary>
        ICollection<IWavesApplicationAction> ApplicationActions { get; }

        /// <summary>
        ///     Event for applications actions updated.
        /// </summary>
        event ApplicationsActionsUpdatedEventHandler ApplicationsActionsUpdated;
    }
}