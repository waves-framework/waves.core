using System.Collections.Generic;

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