using System.Collections.Generic;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    /// Interface for service loader.
    /// </summary>
    public interface IServiceLoader : IMefLoaderService<IService>
    {
        /// <summary>
        ///     Loads services by current service type.
        /// </summary>
        /// <returns>Collection of services.</returns>
        public ICollection<T> GetService<T>();
    }
}