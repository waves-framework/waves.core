using System;

namespace Waves.Core.Base.Interfaces.Services
{
    /// <summary>
    ///     Interface for module service classes.
    /// </summary>
    public interface IModuleService : IMefLoaderService<IWavesModule>
    {
        /// <summary>
        ///     Gets module by ID (Guid).
        /// </summary>
        /// <param name="id">ID.</param>
        IWavesModule GetModule(Guid id);

        /// <summary>
        ///     Gets module by ID (string).
        /// </summary>
        /// <param name="id">ID.</param>
        IWavesModule GetModule(string id);
    }
}