using System;

namespace Waves.Core.Base.Interfaces.Services
{
    /// <summary>
    ///     Interface for module service classes.
    /// </summary>
    public interface IModuleService : IMefLoaderService<IModule>
    {
        /// <summary>
        ///     Gets module by ID (Guid).
        /// </summary>
        /// <param name="id">ID.</param>
        IModule GetModule(Guid id);

        /// <summary>
        ///     Gets module by ID (string).
        /// </summary>
        /// <param name="id">ID.</param>
        IModule GetModule(string id);
    }
}